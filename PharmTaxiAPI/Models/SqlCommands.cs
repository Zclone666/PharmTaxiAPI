using MySqlConnector;
using System.Data;

namespace Core.Data
{
	internal static class SqlCommands
	{
		private const string LibSelectByLevel =
@"SET @sql = CONCAT('SELECT ', CAST(@lib AS CHAR), ' AS lib, id, master_parent, parent_id, depth, sort, level, nature, code, lib_a, id_a, lib_b, id_b, sex, value_a, value_b, value_c, value_d, ratio, confirmed, count, flag FROM lib_', CAST(@lib AS CHAR), ' WHERE level = ', CAST(@level AS CHAR), ';');
PREPARE dynamic_query FROM @sql;
EXECUTE dynamic_query;
DEALLOCATE PREPARE dynamic_query;";

		private const string LibSelectAsLinks =
@"SET @sql = CONCAT('SELECT ', CAST(@lib AS CHAR), ' AS lib, id, master_parent, parent_id, depth, sort, level, nature, code, lib_a, id_a, lib_b, id_b, sex, value_a, value_b, value_c, value_d, ratio, confirmed, count, flag FROM lib_', CAST(@lib AS CHAR), ' WHERE lib_a = ', CAST(@lib_a AS CHAR), ' AND id_a = ', CAST(@id_a AS CHAR), ' AND lib_b = ', CAST(@lib_b AS CHAR), ' AND id_b = ', CAST(@id_b AS CHAR), ';');
PREPARE dynamic_query FROM @sql;
EXECUTE dynamic_query;
DEALLOCATE PREPARE dynamic_query;";

		private const string LibNamesSelect =
@"SET @sql= CONCAT('SELECT ', CAST(@lib AS CHAR), ' AS lib, id, master_id, lang, field, text, weight, popularity FROM lib_', CAST(@lib AS CHAR), '_names WHERE field = ''name'';');
PREPARE dynamic_query FROM @sql;
EXECUTE dynamic_query;
DEALLOCATE PREPARE dynamic_query;";

		private const string LibNamesSelectByMasterId =
@"SET @sql= CONCAT('SELECT ', CAST(@lib AS CHAR), ' AS lib, id, master_id, lang, field, text, weight, popularity FROM lib_', CAST(@lib AS CHAR), '_names WHERE master_id = ', CAST(@master_id AS CHAR), ' AND field = ''name'';');
PREPARE dynamic_query FROM @sql;
EXECUTE dynamic_query;
DEALLOCATE PREPARE dynamic_query;";

		private const string LibDescriptionsByFieldSelect =
@"SET @sql= CONCAT('SELECT ', CAST(@lib AS CHAR), ' AS lib, id, master_id, lang, field, text, weight, source_id FROM lib_', CAST(@lib AS CHAR), '_descriptions WHERE field = ''', CAST(@field AS CHAR), ''';');
PREPARE dynamic_query FROM @sql;
EXECUTE dynamic_query;
DEALLOCATE PREPARE dynamic_query;";

		private const string TableFieldsSelect =
@"SET @dbName = database();

SELECT COLUMN_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_name = @tblName
	AND table_schema = @dbName
ORDER BY ORDINAL_POSITION ASC;";

		private const string TableStructureSelect =
@"SET @dbName = database();

CREATE TEMPORARY TABLE tmp_CONSTRAINTS
SELECT CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_SCHEMA = @dbName
	AND TABLE_NAME = @tblName;

SELECT i.SqlText
FROM
(
	SELECT CONCAT('`', COLUMN_NAME, '` ',
		UPPER(COLUMN_TYPE), ' ',
		IF(IS_NULLABLE = 'NO',
			'NOT NULL ',
			IF(COLUMN_TYPE = 'timestamp',
				'NULL ',
                ''
			)
		),
		IF(IS_NULLABLE = 'YES' OR COLUMN_DEFAULT IS NOT NULL,
			'DEFAULT ',
			''),
		IF(IS_NULLABLE = 'YES' OR COLUMN_DEFAULT IS NOT NULL,
			IF(COLUMN_DEFAULT IS NULL OR COLUMN_DEFAULT = 'NULL',
				'NULL ',
				IF(COLUMN_DEFAULT LIKE '%CURRENT%',
					CONCAT(COLUMN_DEFAULT, ' '),
					IF(COLUMN_DEFAULT LIKE 'b''%',
						CONCAT(CAST(COLUMN_DEFAULT AS CHAR), ' '),
						CONCAT('''', CAST(COLUMN_DEFAULT AS CHAR), ''' ')
					)
				)
			),
			''),
		IF(IFNULL(EXTRA, '') <> '',
			CONCAT(UPPER(EXTRA), ' '),
			''),
		IF(IFNULL(COLUMN_COMMENT, '') <> '',
			CONCAT('COMMENT ''', COLUMN_COMMENT, ''''),
			'')
		) AS SqlText,
		ORDINAL_POSITION AS OrderBy
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_name = @tblName
		AND table_schema = @dbName

	UNION ALL

	SELECT CONCAT('PRIMARY KEY(`',
    (
		SELECT GROUP_CONCAT(`COLUMN_NAME` SEPARATOR '`, `')
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE table_name = @tblName
			AND table_schema = @dbName
			AND COLUMN_KEY = 'PRI'
    ), '`)') AS SqlText,
		200 AS OrderBy

	UNION ALL

	SELECT CONCAT('KEY `', s.INDEX_NAME, '` (',
		(
			SELECT GROUP_CONCAT(DISTINCT CONCAT('`', si.COLUMN_NAME, '`')
				ORDER BY si.SEQ_IN_INDEX
				SEPARATOR ',') str
			FROM INFORMATION_SCHEMA.STATISTICS si
			WHERE si.TABLE_SCHEMA = s.TABLE_SCHEMA
				AND si.TABLE_NAME = s.TABLE_NAME
				AND si.INDEX_NAME = s.INDEX_NAME
		), ')') AS SqlText,
		999 AS OrderBy
	FROM INFORMATION_SCHEMA.STATISTICS s
		LEFT OUTER JOIN tmp_CONSTRAINTS t ON s.INDEX_NAME=t.CONSTRAINT_NAME 
	WHERE t.CONSTRAINT_NAME IS NULL
		AND s.TABLE_SCHEMA = @dbName
		AND s.TABLE_NAME = @tblName
	GROUP BY s.TABLE_NAME, s.INDEX_NAME
) i
ORDER BY i.OrderBy ASC;

DROP TEMPORARY TABLE tmp_CONSTRAINTS;";


		private const string TableStructureEncryptedSelect =
@"SET @dbName = database();

CREATE TEMPORARY TABLE tmp_COLUMNS
SELECT COLUMN_NAME,
	DATA_TYPE,
	CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE table_schema = @dbName
	AND TABLE_NAME = @tblName;

CREATE TEMPORARY TABLE tmp_CONSTRAINTS
SELECT CONSTRAINT_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE TABLE_SCHEMA = @dbName
	AND TABLE_NAME = @tblName;


SELECT i.SqlText
FROM
(
	SELECT CONCAT('`', COLUMN_NAME, '` ',
		IF(COLUMN_NAME = 'id', CONCAT(UPPER(COLUMN_TYPE), ' '), IF(DATA_TYPE LIKE '%CHAR%',
			'BLOB ',
			IF(DATA_TYPE LIKE '%TEXT%',
				'LONGBLOB ',
				'VARBINARY(255) '))),
		IF(IS_NULLABLE = 'NO',
			'NOT NULL ',
			IF(DATA_TYPE LIKE '%CHAR%' OR DATA_TYPE LIKE '%TEXT%',
				' ',
				'DEFAULT NULL ')),
			IF(COLUMN_NAME = 'id', UPPER(EXTRA), '')
		) AS SqlText,
		ORDINAL_POSITION AS OrderBy
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE table_name = @tblName
		AND table_schema = @dbName

	UNION ALL

	SELECT CONCAT('PRIMARY KEY(`',
    (
		SELECT GROUP_CONCAT(`COLUMN_NAME` SEPARATOR '`, `')
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE table_name = @tblName
			AND table_schema = @dbName
			AND COLUMN_KEY = 'PRI'
    ), '`)') AS SqlText,
		200 AS OrderBy

	UNION ALL

	SELECT CONCAT('KEY `', s.INDEX_NAME, '` (',
		(
			SELECT GROUP_CONCAT(DISTINCT CONCAT('`',
					si.COLUMN_NAME,
					'`',
					IF(c.DATA_TYPE LIKE '%CHAR%',
						CONCAT('(', CAST(16 * (FLOOR(c.CHARACTER_MAXIMUM_LENGTH / 16) + 1) AS CHAR), ') '),
						IF(DATA_TYPE LIKE '%TEXT%',
							'(512)',
							'')))
				ORDER BY si.SEQ_IN_INDEX
				SEPARATOR ',') str
			FROM INFORMATION_SCHEMA.STATISTICS si
			INNER JOIN tmp_COLUMNS c ON c.COLUMN_NAME = si.COLUMN_NAME
			WHERE si.TABLE_SCHEMA = s.TABLE_SCHEMA
				AND si.TABLE_NAME = s.TABLE_NAME
				AND si.INDEX_NAME = s.INDEX_NAME

		), ')') AS SqlText,
		999 AS OrderBy
	FROM INFORMATION_SCHEMA.STATISTICS s
		LEFT OUTER JOIN tmp_CONSTRAINTS t ON s.INDEX_NAME=t.CONSTRAINT_NAME 
	WHERE t.CONSTRAINT_NAME IS NULL
		AND s.TABLE_SCHEMA = @dbName
		AND s.TABLE_NAME = @tblName
	GROUP BY s.TABLE_NAME, s.INDEX_NAME
) i
ORDER BY i.OrderBy ASC;

DROP TEMPORARY TABLE tmp_COLUMNS;
DROP TEMPORARY TABLE tmp_CONSTRAINTS;";

		private const string LibStructureSelect =
@"SET @dbName = database();
SET @isVal = CONCAT('lib_', CAST(@lib AS CHAR)),
	@likeVal = CONCAT('lib\_', CAST(@lib AS CHAR), '\_%');

SELECT TABLE_NAME, ENGINE, TABLE_COMMENT
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @dbName
	AND (TABLE_NAME = @isVal OR TABLE_NAME LIKE @likeVal);";

		private const string SettingsStructureSelect =
@"SET @dbName = database();
SET @isVal = @name,
	@likeVal = CONCAT(@name, '\_%');

SELECT TABLE_NAME, ENGINE, TABLE_COMMENT
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @dbName
	AND (TABLE_NAME = @isVal OR TABLE_NAME LIKE @likeVal);";

		private const string DirectStructureSelect =
@"SET @dbName = database();

SELECT TABLE_NAME, ENGINE, TABLE_COMMENT
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @dbName
	AND TABLE_NAME = @name;";

		internal static MySqlCommand CreateTableFieldsSelect()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = TableFieldsSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@tblName", MySqlDbType.VarChar);
			return (cmd);
		}

		internal static MySqlCommand CreateTableStructureSelect(bool encrypted)
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = encrypted
					? TableStructureEncryptedSelect
					: TableStructureSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@tblName", MySqlDbType.VarChar);
			return (cmd);
		}

		internal static MySqlCommand CreateLibStructureSelect()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibStructureSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			return (cmd);
		}

		internal static MySqlCommand CreateSettingsStructureSelect()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = SettingsStructureSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@name", MySqlDbType.VarChar);
			return (cmd);
		}

		internal static MySqlCommand CreateDirectStructureSelect()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = DirectStructureSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@name", MySqlDbType.VarChar);
			return (cmd);
		}

		/// <summary>
		/// Команда выборки данных по lib и level
		/// </summary>
		/// <returns>Команда</returns>
		internal static MySqlCommand CreateLibSelectByLevel()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibSelectByLevel,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			cmd.Parameters.Add("@level", MySqlDbType.UInt16);
			return (cmd);
		}

		/// <summary>
		/// Команда выборки данных по lib и id
		/// </summary>
		/// <returns>Команда</returns>
		internal static MySqlCommand CreateLibSelectAsLinks()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibSelectAsLinks,
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			cmd.Parameters.Add("@lib_a", MySqlDbType.UInt16);
			cmd.Parameters.Add("@id_a", MySqlDbType.UInt32);
			cmd.Parameters.Add("@lib_b", MySqlDbType.UInt16);
			cmd.Parameters.Add("@id_b", MySqlDbType.UInt32);
			return (cmd);
		}

		/// <summary>
		/// Команда выборки всего содержимого таблицы наименований
		/// </summary>
		/// <returns>Команда</returns>
		internal static MySqlCommand CreateLibNamesSelect()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibNamesSelect,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			return (cmd);
		}

		/// <summary>
		/// Команда выборки содержимого таблицы наименований по id
		/// </summary>
		/// <returns>Команда</returns>
		internal static MySqlCommand CreateLibNamesSelectByMasterId()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibNamesSelectByMasterId,
				CommandTimeout = 1800
			};
			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			cmd.Parameters.Add("@master_id", MySqlDbType.UInt32);
			return (cmd);
		}

		/// <summary>
		/// Команда вставки в таблицу similarity (база Service)
		/// </summary>
		/// <returns>Команда (с параметрами @group_md5, @main, @similarity_lib, @similarity_id, @weight, @status)</returns>
		internal static MySqlCommand CreateServiceSimilarityInsert()
		{
			MySqlCommand insertCmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"INSERT INTO `service`.`similarity`(`group_md5`, `main`, `similarity_lib`, `similarity_id`, `weight`, `status`)
VALUES(@group_md5, @main, @similarity_lib, @similarity_id, @weight, @status);",
				CommandTimeout = 1800
			};

			insertCmd.Parameters.Add("@group_md5", MySqlDbType.VarChar, 32);
			insertCmd.Parameters.Add("@main", MySqlDbType.UInt16);
			insertCmd.Parameters.Add("@similarity_lib", MySqlDbType.Int16);
			insertCmd.Parameters.Add("@similarity_id", MySqlDbType.UInt32);
			insertCmd.Parameters.Add("@weight", MySqlDbType.Double);
			insertCmd.Parameters.Add("@status", MySqlDbType.VarChar, 500);

			return (insertCmd);
		}

		internal static MySqlCommand CreateCheckMd5InSinilarity()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SELECT CASE
	WHEN EXISTS(SELECT 1 FROM `service`.`similarity` WHERE group_md5 = @group_md5 LIMIT 1) THEN 1
	ELSE 0
END;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@group_md5", MySqlDbType.VarChar, 32);
			return (cmd);
		}

		internal static MySqlCommand CreateGetAllLibs()
		{
			return (new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SHOW TABLES
FROM `umkb`
LIKE 'lib_%_names';",
				CommandTimeout = 1800
			});
		}

		internal static MySqlCommand CreateGetAllChecks()
		{
			return (new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SELECT *
FROM `service`.`similarity_checks`;",
				CommandTimeout = 1800
			});
		}

		internal static MySqlCommand CreateInsertCheck()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"INSERT INTO `service`.`similarity_checks`(similarity_lib, lastcheck)
VALUES(@similarity_lib, @lastcheck);",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@similarity_lib", MySqlDbType.Int16);
			cmd.Parameters.Add("@lastcheck", MySqlDbType.DateTime);
			return (cmd);
		}

		internal static MySqlCommand CreateUpdateCheck()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"UPDATE `service`.`similarity_checks`
SET lastcheck = @lastcheck
WHERE similarity_lib = @similarity_lib;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@similarity_lib", MySqlDbType.Int16);
			cmd.Parameters.Add("@lastcheck", MySqlDbType.DateTime);
			return (cmd);
		}

		internal static MySqlCommand CreateGetDate()
		{
			return (new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = @"SELECT NOW()",
				CommandTimeout = 1800
			});
		}

		internal static MySqlCommand CreateGetUsers()
		{
			return (new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SELECT *
FROM
(
	SELECT l.id,
		CONCAT(
			IFNULL((SELECT CONCAT(`text`, ' ') FROM umkb.lib_900_names WHERE master_id = l.id AND field = 'last_name' ORDER BY lang DESC LIMIT 1), ''),
			IFNULL((SELECT CONCAT(`text`, ' ') FROM umkb.lib_900_names WHERE master_id = l.id AND field = 'name' ORDER BY lang DESC LIMIT 1), ''),
			IFNULL((SELECT CONCAT(`text`, ' ') FROM umkb.lib_900_names WHERE master_id = l.id AND field = 'middle_name' ORDER BY lang DESC LIMIT 1), '')) AS `name`
	FROM umkb.lib_900 l
) i
ORDER BY `name` ASC",
				CommandTimeout = 1800
			});
		}

		internal static MySqlCommand CreateGetDescriptionsByField()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = LibDescriptionsByFieldSelect,
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@lib", MySqlDbType.UInt16);
			cmd.Parameters.Add("@field", MySqlDbType.VarChar);

			return(cmd);
		}


		internal static MySqlCommand CreateGetPharmsByKey()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
	/*@"SELECT DISTINCT f.id,
		lg.`value` AS login,
		a.master_id AS userid
	FROM users_auth a
		INNER JOIN lib_902 u ON a.master_id IN (u.id, u.parent_id)
		INNER JOIN lib_902_varchar v ON v.master_id = u.id
			AND v.field = 'type'
			AND v.`value` = 'pharmacy'
		INNER JOIN lib_902_varchar lg ON lg.master_id = u.id
			AND lg.field = 'login'
		INNER JOIN lib_920 l ON l.id_a = u.id
			AND l.`level` = 9112
			AND l.lib_a = 902
			AND l.lib_b = 95
		INNER JOIN lib_95 f ON l.id_b IN (f.id, f.parent_id)
	WHERE a.auth_key = @key;",*/
	@"SELECT DISTINCT f.id,
    lg.`value` AS login,
    u.parent_id AS userid
FROM lib_902 u
    INNER JOIN lib_902_varchar v ON v.master_id = u.id
        AND v.field = 'type'
        AND v.`value` = 'pharmacy'
    INNER JOIN lib_902_varchar lg ON lg.master_id = u.id
        AND lg.field = 'login'
    INNER JOIN lib_920 l ON l.id_a = u.id
        AND l.`level` = 9112
        AND l.lib_a = 902
        AND l.lib_b = 95
    INNER JOIN lib_95 f ON l.id_b IN (f.id, f.parent_id)
WHERE u.parent_id =@key or u.id=@key;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@key", MySqlDbType.VarChar, 32);

			return (cmd);
		}

		internal static MySqlCommand CreateAuth()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SET @auth_key = REPLACE(UUID(), '-', '');
INSERT INTO users_auth(master_id, profile, auth_key) VALUES(@master_id, @profile, @auth_key);
SELECT @auth_key;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@master_id", MySqlDbType.UInt32);
			cmd.Parameters.Add("@profile", MySqlDbType.VarChar, 20);

			return (cmd);

		}
		internal static MySqlCommand CreateUnAuth()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = @"DELETE FROM users_auth WHERE auth_key = @auth_key;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@auth_key", MySqlDbType.VarChar, 32);

			return (cmd);

		}

		internal static MySqlCommand CreateSaveDrugScan()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"SET @UserId = (SELECT master_id FROM users_auth WHERE auth_key = @auth_key);

-- Если концепт не известен - будет NULL
SET @BatchId =
(
	SELECT id
    FROM lib_67_dyn_batchs
    WHERE master_id = @Fp_id
		AND master_id > 0
        AND `number` = @Batch
        AND lib_26_id$type = 1998 -- Партия
	LIMIT 1
);

-- Надо ли добавлять новые номера серий?... (возможно только при непустом @Fp_id)

INSERT INTO lib_67_dyn_codes
(
	master_id,
	`serial`,
	batch_lot,
	gtin,
	use_by_or_expiry,
	`format`,
	raw_code,
	`comment`,
	lib_67_dyn_batchs_id$batch
    -- lib_26_id$status = 2049 -- В продаже
	-- validated = 0
	-- authenticity = 0
	-- probability_acquire = 0
	-- probability_seller = 1
	-- scans_count = 1
)
VALUES
(
	@Fp_id,
	@Serial,
	@Batch,
	@GTIN,
	@UseBy,
	@Format,
	@RawCode,
	@Comment,
	@BatchId
)
ON DUPLICATE KEY UPDATE
	master_id = @Fp_id, -- Если до этого ФП не был опознан - сейчас он будет сопоставлен
    lib_67_dyn_batchs_id$batch = @BatchId, -- Если до этого batch не был сопоставлен - сейчас он будет сопоставлен
	scans_count = scans_count + 1;

SET @CodeId = 
(
	SELECT id
	FROM lib_67_dyn_codes
	WHERE gtin = @GTIN
		AND batch_lot = @Batch
		AND `serial` = @Serial
	LIMIT 1
);

INSERT INTO lib_67_dyn_code_events
(
	master_id,
	lib_67_dyn_codes_id,
	lib_902_id,
	latitude,
	longitude,
    accuracy
    -- date = CURRENT_TIMESTAMP
    -- lib_26_id = 2048 -- Сканирование
	-- trust = 1.0
)
VALUES
(
	@Fp_id,
	@CodeId,
	@UserId,
	@Latitude,
	@Longitude,
    @Accuracy
);

-- Если @Fp_id не пустой и такого GTIN нет в lib_67_varchar по field='EAN' - добавить его туда
-- Сделать какие-то вещи со статусом и вероятностями в lib_67_dyn_codes от содержимого lib_67_dyn_code_events по @CodeId",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@GTIN", MySqlDbType.VarChar, 14);
			cmd.Parameters.Add("@Batch", MySqlDbType.VarChar, 20);
    		cmd.Parameters.Add("@Serial", MySqlDbType.VarChar, 20);
   			cmd.Parameters.Add("@UseBy", MySqlDbType.Date);
			cmd.Parameters.Add("@Format", MySqlDbType.VarChar, 50);
			cmd.Parameters.Add("@RawCode", MySqlDbType.VarChar, 8192);
			cmd.Parameters.Add("@Comment", MySqlDbType.VarChar, 300);
    		cmd.Parameters.Add("@Fp_id", MySqlDbType.UInt32);
    		cmd.Parameters.Add("@auth_key", MySqlDbType.VarChar, 32);
    		cmd.Parameters.Add("@Latitude", MySqlDbType.Double);
    		cmd.Parameters.Add("@Longitude", MySqlDbType.Double);
			cmd.Parameters.Add("@Accuracy", MySqlDbType.UInt32);

			return (cmd);
		}

		internal static MySqlCommand CreateAssociateDrugScan()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText =
@"UPDATE lib_67_dyn_codes, lib_67_dyn_code_events
SET lib_67_dyn_codes.master_id = @Fp_id,
	lib_67_dyn_code_events.master_id = @Fp_id
WHERE lib_67_dyn_code_events.lib_67_dyn_codes_id = lib_67_dyn_code_events.id
	AND lib_67_dyn_codes.gtin = @GTIN
	AND lib_67_dyn_codes.master_id = 0;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@GTIN", MySqlDbType.VarChar, 14);
			cmd.Parameters.Add("@Fp_id", MySqlDbType.UInt32);

			return (cmd);

		}

		internal static MySqlCommand CreateGetUserId()
		{
			MySqlCommand cmd = new MySqlCommand()
			{
				CommandType = CommandType.Text,
				CommandText = @"SELECT master_id FROM users_auth WHERE auth_key = @auth_key LIMIT 1;",
				CommandTimeout = 1800
			};

			cmd.Parameters.Add("@auth_key", MySqlDbType.VarChar, 32);

			return (cmd);
		}
	}
}

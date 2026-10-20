using System;
using System.Collections.Generic;
using System.Text;

namespace PharmTaxiAdm.Methods
{
    public static class SQLRequests
    {
        public const string SetCode = @"UPDATE lib_95 set code =@code where id = @id";
        public const string GetByCode = @"SELECT id FROM `lib_95` WHERE code=@code";
        public const string GetByPharmId = @"SELECT code FROM `lib_95` WHERE id=@pharmId";
        public const string GetUserByEmail = @"SELECT master_id FROM `lib_902_varchar` where value = @email and field='email' ORDER by master_id desc";
        public const string CreateUserWithNoPass = "INSERT INTO `lib_902`(`id`, `master_parent`, `parent_id`, `depth`, `sort`, `level`, `nature`, `code`, `lib_a`, `id_a`, `lib_b`, `id_b`, `sex`, `value_a`, `value_b`, `value_c`, `value_d`, `ratio`, `confirmed`, `count`, `flag`, `date_created`, `date_updated`) VALUES ((select max(id)), NULL,2,NULL,0,951,0,@email,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,NULL,0,0,0,1, NOW(), NOW()); " +
            "INSERT INTO `lib_902_names`(`id`, `master_id`, `lang`, `field`, `text`, `weight`, `popularity`, `date_created`, `date_updated`) VALUES ((SELECT max(id)), (select max(id) from lib_902), 'ru', 'name', @email,1,0,NOW(), NOW()); " +
            @"INSERT INTO `lib_902_varchar`(`id`, `master_id`, `field`, `value`, `weight`, `source_id`, `date_created`, `date_updated`) VALUES ((select max(id)),
(select max(id) from lib_902),'email',@email,1, NULL, NOW(), NOW());
        INSERT INTO `lib_902_varchar`(`id`, `master_id`, `field`, `value`, `weight`, `source_id`, `date_created`, `date_updated`) VALUES((select max(id)),
(select max(id) from lib_902),'firstName',@email,1,NULL,NOW(),NOW());
INSERT INTO `lib_902_varchar`(`id`, `master_id`, `field`, `value`, `weight`, `source_id`, `date_created`, `date_updated`) VALUES((select max(id)),
(select max(id) from lib_902),'lastName',@email,1,NULL,NOW(),NOW());
INSERT INTO `lib_902_varchar`(`id`, `master_id`, `field`, `value`, `weight`, `source_id`, `date_created`, `date_updated`) VALUES((select max(id)),
(select max(id) from lib_902),'pass','test',1,NULL,NOW(),NOW());
INSERT INTO `lib_902_varchar`(`id`, `master_id`, `field`, `value`, `weight`, `source_id`, `date_created`, `date_updated`) VALUES((select max(id)),
(select max(id) from lib_902),'verificationKey','test',1,NULL,NOW(),NOW())";

        public const string UpdateUserWithNoPass="UPDATE `lib_902_varchar` SET `value`=@email WHERE master_id=@id and field='email'; " +
            "UPDATE `lib_902_varchar` SET `value`='' WHERE master_id=@id and field='pass'; " +
            "UPDATE `lib_902_names` SET `text`=@email WHERE master_id=@id and field='name'; " +
            "UPDATE `lib_902` SET `code`=@email WHERE id=@id ";
        public const string GetUserConfirCode = "SELECT `value` FROM `lib_902_varchar` WHERE master_id=@id and field='verificationKey';";
        public const string GetUserEmailByUserID = "SELECT `value` FROM `lib_902_varchar` WHERE master_id=@id and field='email';";
        public const string MakingRealSlave="UPDATE `lib_902` SET `code`=@email, parent_id=@parent_id WHERE id=@id ";

    }
}

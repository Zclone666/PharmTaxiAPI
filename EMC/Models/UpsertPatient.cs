using ECPLib.Common.Patient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMC.Models
{
    public class UpsertPatient
    {
        /// <summary>
        /// ИД пользователя
        /// </summary>
        public ulong userId { get; set; }
        /// <summary>
        /// Локаль пользователя
        /// </summary>
        public string active_language { get; set; } = "ru";
        /// <summary>
        /// Пациент
        /// </summary>
        public Patient patient { get; set; }
        /// <summary>
        /// ИД врача
        /// </summary>
        public ulong doctorId { get; set; }
        /// <summary>
        /// Название врача
        /// </summary>
        public string doctorName { get; set; }
        /// <summary>
        /// ИД больницы
        /// </summary>
        public ulong? clinicId { get; set; }
        /// <summary>
        /// Название больницы
        /// </summary>
        public string? clinicName { get; set; }
        /// <summary>
        /// ИД отделения
        /// </summary>
        public ulong? departmentId { get; set; }
        /// <summary>
        /// Название отделения
        /// </summary>
        public string? departmentName { get; set; }
    }
}

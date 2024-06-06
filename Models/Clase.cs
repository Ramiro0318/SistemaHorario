﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorario.Models
{
    [Table("Clase")]
    public class Clase
    {
        [PrimaryKey, AutoIncrement]
        public int Id { set; get; }

        [NotNull]
        public string Nombre { set; get; } = null!;

        [NotNull, MaxLength(3)]
        public string Dia { set; get; } = null!;

        [NotNull]
        public string HoraInicio { set; get; } = null!;

        [NotNull]
        public string HoraFin { set; get; } = null!;

        [NotNull]
        public string Asignatura { set; get; } = null!;

        [NotNull]
        public string Aula { set; get; } = null!;

        [NotNull]
        public string Maestro { set; get; } = null!;
    }
}

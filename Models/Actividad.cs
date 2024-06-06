using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace SistemaHorario.Models
{

    [Table("Actividad")]
    public class Actividad
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

        [NotNull, MaxLength(255)]
        public string Descripcion { set; get; } = null!;

    }
}

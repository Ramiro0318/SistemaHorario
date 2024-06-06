using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using SistemaHorario.Models;
using SistemaHorario.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHorario.Repositories
{
    public class ActividadRespository
    {
        SQLiteConnection conexion;

        public ActividadRespository()
        {
            //conexion = new("data/actividades.sqlite");
            var ruta = "actividades.sqlite";
            conexion = new SQLiteConnection(ruta);
            conexion.CreateTable<Actividad>();
        }

        public IEnumerable<Actividad> GetAll()
        {
            return conexion.Table<Actividad>().OrderBy(x => x.HoraInicio);
        }
        public IEnumerable<Actividad> GetDay(string dia)
        {
            return conexion.Table<Actividad>().Where(x => x.Dia == dia);
        }

        public void Insert(Actividad actividad)
        {
            conexion.Insert(actividad);
        }

        public void Update(Actividad actividad) 
        { 
            conexion.Update(actividad);
        }

        public void Delete(Actividad actividad) 
        {
            conexion.Delete(actividad);
        }
    }
}

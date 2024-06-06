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
        SQLiteConnection conexionA;

        public ActividadRespository()
        {
            var ruta = "actividades.sqlite";
            conexionA = new SQLiteConnection(ruta);
            conexionA.CreateTable<Actividad>();
        }

        public IEnumerable<Actividad> GetAll()
        {
            return conexionA.Table<Actividad>().OrderBy(x => x.HoraInicio);
        }
        public IEnumerable<Actividad> GetDay(string dia)
        {
            return conexionA.Table<Actividad>().Where(x => x.Dia == dia).OrderBy(x => x.HoraInicio);
        }

        public void Insert(Actividad actividad)
        {
            conexionA.Insert(actividad);
        }

        public void Update(Actividad actividad) 
        { 
            conexionA.Update(actividad);
        }

        public void Delete(Actividad actividad) 
        {
            conexionA.Delete(actividad);
        }
    }
}

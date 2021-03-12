using APIRest_Geo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APIRest_Geo.Services
{
    public class GeoRepository
    {
        public List<Geo> Get()
        {
            using (var db = new ApplicationDbContext())
                return db.Geo.ToList();
        }
        public Geo Get(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var res = db.Geo.Where(x => x.Id == id);
                return res == null ? null : res.First();
            }
        }

        internal int Insert(Geo model)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Geo.Add(model);
                db.SaveChanges();
                return model.Id;
            }
        }

        internal void Update(Geo model)
        {
            using (var db = new ApplicationDbContext())
            {
                var g = db.Geo.SingleOrDefault(x => x.Id == model.Id);
                if(g != null)
                {
                    g.Latitud = model.Latitud;
                    g.Longitud = model.Longitud;
                    g.Estado = model.Estado;
                    db.SaveChanges();
                }
            }
        }
    }
}

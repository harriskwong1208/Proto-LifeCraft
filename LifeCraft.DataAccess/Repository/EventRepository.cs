using LifeCraft.DataAccess.Data;
using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeCraft.DataAccess.Repository
{
    public class EventRepository : Repository<Event> , IEventRepository
    {
        private  ApplicationDbContext _db;
        public EventRepository(ApplicationDbContext db) : base(db) 
        { 
            _db = db;
        }
        public void Update(Event obj)
        {
            var objFromDb = _db.Events.FirstOrDefault(u => u.Id == obj.Id);
            if (objFromDb != null)
            {
				objFromDb.Name = obj.Name;
				objFromDb.Description = obj.Description;
				objFromDb.CategoryId = obj.CategoryId;
                objFromDb.Date = obj.Date;
				if (obj.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
			}
            {
                
            }
        }
    }
}

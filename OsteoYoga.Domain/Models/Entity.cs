using System.Web.Mvc;
using OsteoYoga.Domain.Models.Interface;

namespace OsteoYoga.Domain.Models
{
    public class Entity : IEntity
    {
        public virtual int Id { get; set; }
    }
}

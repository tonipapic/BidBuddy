using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EntitiesLayer.Entities
{
    public class Filter<T>
    {
        private List<Expression<Func<T, bool>>> filters = new List<Expression<Func<T, bool>>>();
        public List<Expression<Func<T, bool>>> Filters { get => filters; }
        public void AddFilter(Expression<Func<T, bool>> filter) => filters.Add(filter);

    }
}

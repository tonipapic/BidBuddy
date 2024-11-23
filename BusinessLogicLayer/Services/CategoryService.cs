using BusinessLogicLayer.Exceptions;
using DataAccessLayer.Repositories;
using EntitiesLayer.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Services {
    public class CategoryService {
        /// <summary>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        public CategoryService() { }
        /// <summary>
        /// <para>Fetch all <see cref="Category"/> from repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <returns>List of <see cref="Category"/></returns>
        public List<Category> GetAllCategories() {
            using (var repo = new CategoryRepository()) {

                var categories = repo.GetRootCategories().ToList();
                var queue = new Queue<Category>();

                categories.ForEach((c) => queue.Enqueue(c));
                while(queue.Count > 0) {
                    var category = queue.Dequeue();
                    var subCategories = category.SubCategories.ToList();
                    subCategories.ForEach((c) => queue.Enqueue(c));
                }

                return categories;
            }
        }
        /// <summary>
        /// <para>Adds given <paramref name="category"/> to repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="category"></param>
        public void AddCategory(Category category) {
            using (var repo = new CategoryRepository()) {
                repo.Add(category);
            }
        }
        /// <summary>
        /// <para>Updates given <paramref name="category"/> in repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="category"></param>
        public void UpdateCategory(Category category) {
            using (var repo = new CategoryRepository()) {
                repo.Update(category);
            }
        }

        /// <summary>
        /// <para>Deletes given <paramref name="category"/> from repository</para>
        /// <remarks>Josip Mojzeš</remarks>
        /// </summary>
        /// <param name="category"></param>
        /// <exception cref="DatabaseException"></exception>
        public void DeleteCategory(Category category) {

            if (category.HasSubCategories) {
                throw new DatabaseException("Za brisanje kategorije, kategorija ne smije imati podkategrije!");
            }

            using (var repo = new CategoryRepository()) {
                repo.Remove(category);
            }
        }

    }
}

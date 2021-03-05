using ASC.Entities;
using System.Collections.Generic;

namespace ACS.DAL.Repository.Interfaces
{
    public interface IModelRepository
    {
        IEnumerable<Model> GetModels();
        Model GetModel(int id);
        string CreateModel(Model model);
        string EditModel(Model model);
        string DeleteModel(int id);
    }
}

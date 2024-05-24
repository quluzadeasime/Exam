using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Core.RepositoryAbstracts;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concretes
{
    public class ChefService : IChefService
    {
        IChefRepository _chefRepository;
        IWebHostEnvironment _environment;
        public ChefService(IChefRepository chefRepository, IWebHostEnvironment environment = null)
        {
            _chefRepository = chefRepository;
            _environment = environment;
        }

        public void Add(Chef chef)
        {
            if (chef == null) throw new ChefNullException("", "Chef null ola bilmez");
            if (chef.PhotoFile == null) throw new PhotoNullException("PhotoFile", "Photo null ola bilmez");
            if (!chef.PhotoFile.ContentType.Contains("image/")) throw new ContentTypeException("PhotoFile", "Faylin tipi sehvdir");
            if (chef.PhotoFile.Length > 2097152) throw new FileSizeException("PhotoFile", "Max olcu 2 mb ola biler");

            string path = _environment.WebRootPath + @"\uploads\" + chef.PhotoFile.FileName;

            using(FileStream file = new FileStream(path, FileMode.Create))
            {
                chef.PhotoFile.CopyTo(file);
            }
            chef.ImgUrl = chef.PhotoFile.FileName;

            _chefRepository.Add(chef);
            _chefRepository.Commit();

        }

        public void Delete(int id)
        {
            var existChef = _chefRepository.Get(x=>x.Id == id);
            if (existChef == null) throw new ChefNotFoundException("", " Bu Id'li Chef yoxdur");
            string path = _environment.WebRootPath + @"\uploads\" + existChef.ImgUrl;

            if (!File.Exists(path)) throw new Exceptions.FileNotFoundException("", "Fayl yoxdur");

            File.Delete(path);
            _chefRepository.Delete(existChef);
            _chefRepository.Commit();

        }

        public Chef Get(Func<Chef, bool> func = null)
        {
            return _chefRepository.Get(func);
        }

        public List<Chef> GetAll(Func<Chef, bool> func = null)
        {
            return _chefRepository.GetAll(func);
        }

        public void Update(int id, Chef chef)
        {
            if (chef == null) throw new ChefNullException("", "Chef null ola bilmez");

            var existChef = _chefRepository.Get(x => x.Id == id);
            if (existChef == null) throw new ChefNotFoundException("", " Bu Id'li Chef yoxdur");

            if(chef.PhotoFile != null)
            {
                if (!chef.PhotoFile.ContentType.Contains("image/")) throw new ContentTypeException("PhotoFile", "Faylin tipi sehvdir");
                if (chef.PhotoFile.Length > 2097152) throw new FileSizeException("PhotoFile", "Max olcu 2 mb ola biler");

                string path = _environment.WebRootPath + @"\uploads\" + chef.PhotoFile.FileName;

                using (FileStream file = new FileStream(path, FileMode.Create))
                {
                    chef.PhotoFile.CopyTo(file);
                }
                chef.ImgUrl = chef.PhotoFile.FileName;
                existChef.ImgUrl = chef.ImgUrl;
            }
            existChef.Fullname = chef.Fullname;
            existChef.Description = chef.Description;

            _chefRepository.Commit();

        }
    }
}

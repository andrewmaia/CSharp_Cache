using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace CSharp_Cache.Controllers
{
    [ApiController]    
    [Route("[controller]")]    
    public class CarController :  ControllerBase
    {
        private readonly IMemoryCache _memoryCache;        
        const string key="cars";

        public CarController(IMemoryCache memoryCache )
        {
            _memoryCache= memoryCache;
        }


        public ActionResult<List<Car>> Get()
        {
            if (!_memoryCache.TryGetValue(key, out List<Car> cars))
            {
                cars = GetCarsFromDatabase();
                _memoryCache.Set(key, cars);
                Console.WriteLine("Getting cars from database");
            }            
            else
            {
                Console.WriteLine("Getting cars from cache");
            }

            return cars;
        }

        [HttpGet("EvictCache")]
        public ActionResult<string> EvictCache()
        {
            _memoryCache.Remove(key);
            return "Cache evicted ";
        }


        private List<Car> GetCarsFromDatabase(){
            List<Car> l = new List<Car>();
            l.Add(new Car("Polo"));
            l.Add(new Car("Celta"));
            l.Add(new Car("Versa"));
            return l;
        }
    }

    public  record Car(string Name)
    {

    }
}
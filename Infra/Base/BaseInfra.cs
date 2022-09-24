namespace Infra.Base
{
    public class BaseInfra<T> where T : class, new()
    {
        private readonly T objRestaurant;
        protected T RestauranteDb { get => objRestaurant;}

        public BaseInfra()
        {
            objRestaurant = new T();
        }

    }
}

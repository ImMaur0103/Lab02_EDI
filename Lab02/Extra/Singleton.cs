using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab02.Extra
{
    public sealed class Singleton
    {
        private readonly static Singleton instance = new Singleton();
        //public ListaDoble<Jugador> listaDoble;

        private Singleton()
        {
            //listaDoble = new ListaDoble<Jugador>();
        }

        public static Singleton Instance
        {
            get
            {
                return instance;
            }
        }
    }
}

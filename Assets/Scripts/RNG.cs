using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

public class RNG : Random
{
    private static RNG instance = null;

    private RNG() : base() { } 

    public static RNG GetInstance()
    {
        if (instance == null)
        {
            instance = new RNG();
        }
        return instance;
    }    
}

using System;
using System.Collections.Generic;
using System.Linq;
#if Colore
using Corale.Colore.Core;
#endif


namespace RazerChromaTools
{
    public class KeyboardTools
    {
        public KeyboardTools()
        {
        }

        public void SetColorToAllKeys()
        {
            Keyboard.Instance.SetAll(Color.Black);
        }

    }
}

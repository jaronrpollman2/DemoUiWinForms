using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoUiWinForms
{
    //No Longer Used as the other method was more effective and easier
    class MultiKeyPress
    {
        private List<Keys> Keys { get; set; }
        private Keys Modifiers { get; set; }

        public MultiKeyPress(IEnumerable<Keys> keys, Keys modifiers)
        {
            Keys = new List<Keys>(keys);
            Modifiers = modifiers;

            if (Keys.Count <= 1)
            {
                throw new ArgumentException("More than one key must be specified");
            }
        }

        private int i;
        public bool CompareKeypress(KeyEventArgs e)
        {
            
            if(e.Modifiers == Modifiers && Keys[i] == e.KeyCode)
            {
                i++;
            }
            else
            {
                i = 0;
            }

            if (i + 1 > Keys.Count)
            {
                i = 0;
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide.Entity
{
    public class Message
    {
        public int IntData
        {
            get
            {
                return new Random().Next();
            }
        }
        public string StringData
        {
            get
            {
                byte[] bytes = new byte[10];
                new Random().NextBytes(bytes);
                return Encoding.ASCII.GetString(bytes);
            }
        }
        public float FloatData
        {
            get
            {
                return (float)new Random().NextDouble();
            }
        }
    }
}

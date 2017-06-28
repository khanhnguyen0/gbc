using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

interface ISaveable
{
    void saveData(StreamWriter writer);
    void loadData(StreamReader reader);
}
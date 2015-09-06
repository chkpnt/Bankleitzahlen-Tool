using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Bankleitzahlen.Lizenzen
{
    public class GenutzteProjekte
    {
        public IEnumerable<GenutztesProjekt> Projekte { get; private set; }

        public GenutzteProjekte()
        {
            Projekte = GetProjekteFromResources();
        }

        private IEnumerable<GenutztesProjekt> GetProjekteFromResources()
        {
            var deserializer = new Deserializer();
            var lizenzResourcen = from resource in Assembly.GetExecutingAssembly().GetManifestResourceNames()
                                  where resource.StartsWith("Bankleitzahlen.Lizenzen.Projekte") && resource.EndsWith(".yaml")
                                  select resource;

            foreach (var lizenzResource in lizenzResourcen)
            {
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(lizenzResource))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        yield return deserializer.Deserialize<GenutztesProjekt>(streamReader);
                    }
                }
            }
        }
    }
}

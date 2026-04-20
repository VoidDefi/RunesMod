using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunesMod.ModUtils
{
    public class AutoAsset<T> where T : class
    {
        public string Path { get; private set; }

        public string Name { get; private set; }

        private Asset<T> asset;

        public Asset<T> Asset
        {
            get
            {
                if (asset == null)
                    asset = ModAssets.Request<T>(Path, Name);

                return asset;
            }
        }

        public T Value => Asset.Value;

        public AutoAsset(string path, string name)
        {
            Path = path;
            Name = name;
        }
    }
}

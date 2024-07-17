using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : Singleton<GlobalVariables>
{
   public List<GLTFast.GltfImport> objects_app = new List<GLTFast.GltfImport>();
   public List<Texture2D> texture2Ds= new List<Texture2D>();
   
}

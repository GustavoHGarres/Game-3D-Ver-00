using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cloth;

namespace Cloth
{

      public class ClothChanger : MonoBehaviour
      {

           public SkinnedMeshRenderer mesh;

           public Texture2D texture;
           public string shaderIdName = "_EmissionMap";

           private Texture2D _defaultTexture;

           public void Awake()
           {
                _defaultTexture = (Texture2D) mesh.sharedMaterials[0].GetTexture(shaderIdName);
           }

           [NaughtyAttributes.Button]
           private void ChangeTexture()
           {
                mesh.materials[0].SetTexture(shaderIdName, texture);
           }

           public void ChangeTexture(ClothSetup setup)
           {
                mesh.materials[0].SetTexture(shaderIdName, setup.texture);
           }
           
           [NaughtyAttributes.Button]
           public void ResetTexture()
           {
                mesh.materials[0].SetTexture(shaderIdName, _defaultTexture);
           }
      }
}

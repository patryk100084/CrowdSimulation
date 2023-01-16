using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Linq;

public class UnitTests {

	GameObject go;
	
	[UnityTest]
	public IEnumerator TestIDGenerator() {
		System.Collections.Generic.List<string> ids;
		go = new GameObject();
		ids = new System.Collections.Generic.List<string>();

		for(int i = 0; i < 1000; i++)
		{
			IDGenerator idGen = go.AddComponent<IDGenerator>();
			idGen.enabled = true;

			yield return new WaitForFixedUpdate();

			string id = idGen.GetID();
			ids.Add(id);
		}
		Assert.AreEqual(ids.Distinct().Count(), ids.Count());
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CrystalCounter : MonoBehaviour
{
	[Header("Properties")]
	[SerializeField, Tooltip("Objects that will be activated once all the crystals are destroyed (need to implement IActivable interface)")] GameObject[] activables;
	[SerializeField] List<Crystal> crystals = new();
	[SerializeField] Slider sliderBar;

	private int _total;
	private int _current;

	private void Start()
	{
		_total = crystals.Count;
		_current = _total;
		for (int i = 0; i < _total; i++)
		{
			crystals[i].SetCounter(this);
		}
	}

	public void RemoveCrystal(Crystal crystal)
	{
		crystals.Remove(crystal);
		_current--;
		sliderBar.value = (float)((float)_current / (float)_total);

		if (_current <= 0)
		{
			for (int i = 0; i < activables.Length; i++)
				activables[i].GetComponentInChildren<IActivable>().Activate();
		}
	}
}

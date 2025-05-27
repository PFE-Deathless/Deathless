using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CrystalCounter : MonoBehaviour
{
	[Header("Properties")]
	//[SerializeField, Tooltip("Objects that will be activated once all the crystals are destroyed (need to implement IActivable interface)")] GameObject[] activables;
	[SerializeField, Tooltip("All the crystals references for the slider bar")] List<Crystal> crystals = new();
	[SerializeField, Tooltip("Objects that will be activated after a certain milestone is reached (amount of crystal destroyed, need to implement IActivable interface)")] List<CrystalMilestone> milestones = new(); 
	[SerializeField] Slider sliderBar;

	private int _total;
	private int _current;
	private List<CrystalMilestone> _milestoneToRemove = new();

	private void Start()
	{
		_total = crystals.Count;
		_current = _total;
		for (int i = 0; i < _total; i++)
			crystals[i].SetCounter(this);
	}

	public void RemoveCrystal(Crystal crystal)
	{
		crystals.Remove(crystal);
		_current--;
		sliderBar.value = (float)((float)_current / (float)_total);

		// Activate all the milestone objects that reached the threshold
		for (int i = 0; i < milestones.Count; i++)
		{
			if (_total - _current >= milestones[i].threshold)
			{
				for (int j = 0; j < milestones[i].activables.Length; j++)
					milestones[i].activables[j].GetComponentInChildren<IActivable>().Activate();

				_milestoneToRemove.Add(milestones[i]);
			}
		}

		foreach (CrystalMilestone m in _milestoneToRemove)
			milestones.Remove(m);
		_milestoneToRemove.Clear();
	}

	[Serializable]
	class CrystalMilestone
	{
		[SerializeField, Tooltip("")] public int threshold;
		[SerializeField, Tooltip("Game Objects to activate after the amount specified above has been activated")] public GameObject[] activables;
	}
}

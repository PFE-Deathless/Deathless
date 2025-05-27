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

	[Header("Technical")]
	[SerializeField] Slider sliderBar;
	[SerializeField] GameObject thresholdIndicatorPrefab;

	private int _total;
	private int _current;
	private List<CrystalMilestone> _milestoneToRemove = new();

	private void Start()
	{
		_total = crystals.Count;
		_current = _total;
		for (int i = 0; i < _total; i++)
			crystals[i].SetCounter(this);

		PlaceThresholdIndicators();
	}

	void PlaceThresholdIndicators()
	{
		for (int i = 0; i < milestones.Count; i++)
		{
			if (milestones[i].threshold == _total)
				return;

			RectTransform rt = (RectTransform)sliderBar.transform;
			float delta = rt.rect.width / _total;
			float offset = (rt.rect.width / 2f) - (delta * milestones[i].threshold);

			Vector3 pos = sliderBar.transform.position;
			pos.x += offset;

			Instantiate(thresholdIndicatorPrefab, pos, Quaternion.identity, transform);
		}

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

using UnityEngine;

public interface IActivable
{
	public bool FinishedActivation { get; set; }

	public void Activate();

	public Transform transform => transform;
}

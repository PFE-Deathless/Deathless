using UnityEngine;

public interface IActivable
{
	public bool FinishedActivation { get; set; }

	public void Activate(bool playAnimation = true);

	public Transform transform => transform;
}

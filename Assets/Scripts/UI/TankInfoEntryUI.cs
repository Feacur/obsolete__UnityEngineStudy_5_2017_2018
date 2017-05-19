using UnityEngine;
using UnityEngine.UI;

///
/// Tank info entry UI representation
///
public class TankInfoEntryUI : MonoBehaviour {
	public Image image;
	public Text text1;
	public Text text2;

	public void SetText(string text1, string text2) {
		this.text1.text = text1;
		this.text2.text = text2;
	}
}

using UnityEngine;

public class RidingCylinder : MonoBehaviour
{
    private bool _filled; // Þu an kullanýlmýyor (ileride kullanýlabilir)
    private float _value; // Bu silindirin hacmi (0 ile 1 arasýnda olabilir, 1 olursa yeni silindir oluþturur)

    public void IncrementCylinderVolume(float value)
    {
        _value += value; // Silindirin hacmini deðiþtir (arttýr veya azalt)

        // Eðer silindirin hacmi 1’den büyükse
        if (_value > 1)
        {
            float leftValue = _value - 1; // Fazla olan hacim

            // Silindir sayýsýný alýyoruz
            int cylinderCount = PlayerController.Current.ridingCylinders.Count;

            // Bu silindirin Y eksenindeki pozisyonunu ayarla
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f, transform.localPosition.z);

            // Silindirin X-Z eksenindeki geniþliðini sabitliyoruz
            transform.localScale = new Vector3(0.5f, transform.localScale.y, 0.5f);

            // Kalan deðeri yeni bir silindire aktar
            PlayerController.Current.CreateCylinder(leftValue);
        }
        // Eðer silindirin hacmi sýfýrýn altýna düþtüyse
        else if (_value < 0)
        {
            PlayerController.Current.DestroyCylinder(this); // Bu silindiri yok et
        }
        // Normal durumda (0 ile 1 arasýndaysa)
        else
        {
            int cylinderCount = PlayerController.Current.ridingCylinders.Count;

            // Silindirin yüksekliðini ve pozisyonunu hacme göre ayarlýyoruz
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f * _value, transform.localPosition.z);

            // Silindirin geniþliðini hacme göre küçültüp büyütüyoruz
            transform.localScale = new Vector3(0.5f * _value, transform.localScale.y, 0.5f * _value);
        }
    }
}

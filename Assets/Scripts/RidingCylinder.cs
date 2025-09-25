using UnityEngine;

public class RidingCylinder : MonoBehaviour
{
    private bool _filled; // �u an kullan�lm�yor (ileride kullan�labilir)
    private float _value; // Bu silindirin hacmi (0 ile 1 aras�nda olabilir, 1 olursa yeni silindir olu�turur)

    public void IncrementCylinderVolume(float value)
    {
        _value += value; // Silindirin hacmini de�i�tir (artt�r veya azalt)

        // E�er silindirin hacmi 1�den b�y�kse
        if (_value > 1)
        {
            float leftValue = _value - 1; // Fazla olan hacim

            // Silindir say�s�n� al�yoruz
            int cylinderCount = PlayerController.Current.ridingCylinders.Count;

            // Bu silindirin Y eksenindeki pozisyonunu ayarla
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f, transform.localPosition.z);

            // Silindirin X-Z eksenindeki geni�li�ini sabitliyoruz
            transform.localScale = new Vector3(0.5f, transform.localScale.y, 0.5f);

            // Kalan de�eri yeni bir silindire aktar
            PlayerController.Current.CreateCylinder(leftValue);
        }
        // E�er silindirin hacmi s�f�r�n alt�na d��t�yse
        else if (_value < 0)
        {
            PlayerController.Current.DestroyCylinder(this); // Bu silindiri yok et
        }
        // Normal durumda (0 ile 1 aras�ndaysa)
        else
        {
            int cylinderCount = PlayerController.Current.ridingCylinders.Count;

            // Silindirin y�ksekli�ini ve pozisyonunu hacme g�re ayarl�yoruz
            transform.localPosition = new Vector3(transform.localPosition.x, -0.5f * (cylinderCount - 1) - 0.25f * _value, transform.localPosition.z);

            // Silindirin geni�li�ini hacme g�re k���lt�p b�y�t�yoruz
            transform.localScale = new Vector3(0.5f * _value, transform.localScale.y, 0.5f * _value);
        }
    }
}

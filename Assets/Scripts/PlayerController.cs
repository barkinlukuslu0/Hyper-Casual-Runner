using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current; // Singleton gibi davran�yor, ba�ka scriptler buradan PlayerController'a eri�iyor

    public float limitX; // Oyuncunun sa�a sola hareket ederken s�n�rlanaca�� X eksenindeki limit
    public float runningSpeed; // Oyuncunun ileriye do�ru ko�ma h�z� (Z ekseni)
    public float xSpeed; // Oyuncunun sa�-sol hareket etme h�z�
    private float _currentRunningSpeed; // Anl�k ko�ma h�z� (ba�lang��ta -runningSpeed oluyor)

    public GameObject ridingCylinderPrefab; // Alt�na eklenen silindir prefab'�
    public List<RidingCylinder> ridingCylinders; // Oyuncunun alt�nda bulunan silindirlerin listesi

    void Start()
    {
        Current = this; // Current de�i�kenine bu script'i at�yoruz
        _currentRunningSpeed = -runningSpeed; // Oyuncu s�rekli ileri do�ru hareket ediyor (Z ekseninde eksiye gidiyor)
    }

    void Update()
    {
        float newX = 0; // Yeni X pozisyonu
        float touchDeltaX = 0; // Ekrana dokunma ya da mouse hareketinden gelen X fark�

        // Dokunmatik cihazda parma�� kayd�rma kontrol�
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchDeltaX = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        // Mouse ile kontrol
        else if (Input.GetMouseButton(0))
        {
            touchDeltaX = Input.GetAxis("Mouse X");
        }

        // Yeni X pozisyonu hesaplan�yor
        newX = transform.position.x + xSpeed * touchDeltaX * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX); // X pozisyonu limitlerin d���na ��kmas�n

        // Yeni pozisyonu Z ekseninde ileri hareket ederek g�ncelliyoruz
        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + _currentRunningSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // E�er "AddCylinder" tag'li objeye �arparsa
        if (other.tag == "AddCylinder")
        {
            IncrementCylinderVolume(0.1f); // Silindirin hacmini 0.1 artt�r
            Destroy(other.gameObject); // Objeyi yok et
        }
    }

    // Silindirin hacmini art�rma ya da azaltma fonksiyonu
    public void IncrementCylinderVolume(float value)
    {
        if (ridingCylinders.Count == 0) // E�er hi� silindir yoksa
        {
            if (value > 0)
            {
                CreateCylinder(value); // Yeni silindir olu�tur
            }
            else
            {
                // game over
            }
        }
        else
        {
            // Son eklenen silindirin hacmini artt�r/azalt
            ridingCylinders[ridingCylinders.Count - 1].IncrementCylinderVolume(value);
        }
    }

    // Yeni silindir olu�turma fonksiyonu
    public void CreateCylinder(float value)
    {
        // Prefab'dan yeni silindir olu�turuluyor ve Player'�n alt�na ekleniyor
        RidingCylinder createdRidingCylinder = Instantiate(ridingCylinderPrefab, transform).GetComponent<RidingCylinder>();

        // Listeye ekleniyor
        ridingCylinders.Add(createdRidingCylinder);

        // Olu�turulan silindirin hacmi art�r�l�yor
        createdRidingCylinder.IncrementCylinderVolume(value);
    }

    // Var olan silindiri yok etme fonksiyonu
    public void DestroyCylinder(RidingCylinder ridingCylinder)
    {
        ridingCylinders.Remove(ridingCylinder); // Listeden ��kar
        Destroy(ridingCylinder.gameObject); // Objesini yok et
    }
}

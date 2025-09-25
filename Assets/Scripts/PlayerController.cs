using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Current; // Singleton gibi davranýyor, baþka scriptler buradan PlayerController'a eriþiyor

    public float limitX; // Oyuncunun saða sola hareket ederken sýnýrlanacaðý X eksenindeki limit
    public float runningSpeed; // Oyuncunun ileriye doðru koþma hýzý (Z ekseni)
    public float xSpeed; // Oyuncunun sað-sol hareket etme hýzý
    private float _currentRunningSpeed; // Anlýk koþma hýzý (baþlangýçta -runningSpeed oluyor)

    public GameObject ridingCylinderPrefab; // Altýna eklenen silindir prefab'ý
    public List<RidingCylinder> ridingCylinders; // Oyuncunun altýnda bulunan silindirlerin listesi

    void Start()
    {
        Current = this; // Current deðiþkenine bu script'i atýyoruz
        _currentRunningSpeed = -runningSpeed; // Oyuncu sürekli ileri doðru hareket ediyor (Z ekseninde eksiye gidiyor)
    }

    void Update()
    {
        float newX = 0; // Yeni X pozisyonu
        float touchDeltaX = 0; // Ekrana dokunma ya da mouse hareketinden gelen X farký

        // Dokunmatik cihazda parmaðý kaydýrma kontrolü
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchDeltaX = Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        // Mouse ile kontrol
        else if (Input.GetMouseButton(0))
        {
            touchDeltaX = Input.GetAxis("Mouse X");
        }

        // Yeni X pozisyonu hesaplanýyor
        newX = transform.position.x + xSpeed * touchDeltaX * Time.deltaTime;
        newX = Mathf.Clamp(newX, -limitX, limitX); // X pozisyonu limitlerin dýþýna çýkmasýn

        // Yeni pozisyonu Z ekseninde ileri hareket ederek güncelliyoruz
        Vector3 newPosition = new Vector3(newX, transform.position.y, transform.position.z + _currentRunningSpeed * Time.deltaTime);
        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Eðer "AddCylinder" tag'li objeye çarparsa
        if (other.tag == "AddCylinder")
        {
            IncrementCylinderVolume(0.1f); // Silindirin hacmini 0.1 arttýr
            Destroy(other.gameObject); // Objeyi yok et
        }
    }

    // Silindirin hacmini artýrma ya da azaltma fonksiyonu
    public void IncrementCylinderVolume(float value)
    {
        if (ridingCylinders.Count == 0) // Eðer hiç silindir yoksa
        {
            if (value > 0)
            {
                CreateCylinder(value); // Yeni silindir oluþtur
            }
            else
            {
                // game over
            }
        }
        else
        {
            // Son eklenen silindirin hacmini arttýr/azalt
            ridingCylinders[ridingCylinders.Count - 1].IncrementCylinderVolume(value);
        }
    }

    // Yeni silindir oluþturma fonksiyonu
    public void CreateCylinder(float value)
    {
        // Prefab'dan yeni silindir oluþturuluyor ve Player'ýn altýna ekleniyor
        RidingCylinder createdRidingCylinder = Instantiate(ridingCylinderPrefab, transform).GetComponent<RidingCylinder>();

        // Listeye ekleniyor
        ridingCylinders.Add(createdRidingCylinder);

        // Oluþturulan silindirin hacmi artýrýlýyor
        createdRidingCylinder.IncrementCylinderVolume(value);
    }

    // Var olan silindiri yok etme fonksiyonu
    public void DestroyCylinder(RidingCylinder ridingCylinder)
    {
        ridingCylinders.Remove(ridingCylinder); // Listeden çýkar
        Destroy(ridingCylinder.gameObject); // Objesini yok et
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Formless.Player.Rebirth
{
    public class RebirthTimer
    {
        private Image _cooldownImage;
        private float _cooldownTime;
        private float _cooldownTimer;
        private bool _isCooldown;

        public RebirthTimer(Image cooldownImage, float cooldownTime)
        {
            _cooldownImage = cooldownImage;
            _cooldownTime = cooldownTime;
            _cooldownTimer = 0f;
            _isCooldown = false;
        }

        public void UpdateTimer(float deltaTime)
        {
            if (_isCooldown)
            {
                _cooldownTimer -= deltaTime;
                _cooldownImage.fillAmount = Mathf.Clamp01(1 - (_cooldownTimer / _cooldownTime));

                if (_cooldownTimer <= 0)
                {
                    _isCooldown = false;
                    _cooldownImage.fillAmount = 1f;
                }
            }
        }

        public void StartCooldown()
        {
            _isCooldown = true;
            _cooldownTimer = _cooldownTime;
            _cooldownImage.fillAmount = 0f;
        }

        public bool IsCooldownOver()
        {
            return !_isCooldown;
        }
    }
}

//using UnityEngine;
//using UnityEngine.UI;


//namespace Formless.Player.Rebirth
//{
//    public class RebirthTimer
//    {
//        private Image _cooldownImage;
//        private float _cooldownTime;
//        private float _cooldownTimer;
//        private bool _isCooldown;

//        public RebirthTimer(Image cooldownImage, float cooldownTime)
//        {
//            _cooldownImage = cooldownImage;
//            _cooldownTime = cooldownTime;
//            _cooldownTimer = 0f;
//            _isCooldown = false;
//        }

//        public void UpdateTimer(float deltaTime)
//        {
//            if (_isCooldown)
//            {
//                _cooldownTimer -= deltaTime;
//                _cooldownImage.fillAmount = 1 - (_cooldownTimer / _cooldownTime);

//                if (_cooldownTimer <= 0)
//                {
//                    _isCooldown = false;
//                    _cooldownImage.fillAmount = 1f; // Индикатор полностью заполнен
//                }
//            }
//        }

//        public void UpdateTimerReverseFill(float deltaTime)
//        {
//            if (_isCooldown)
//            {
//                _cooldownTimer -= deltaTime;
//                _cooldownImage.fillAmount = _cooldownTimer / _cooldownTime;

//                if (_cooldownTimer <= 0)
//                {
//                    _isCooldown = false;
//                    _cooldownImage.fillAmount = 0f; // Индикатор полностью пуст
//                }
//            }
//        }

//        public void StartCooldown()
//        {
//            _isCooldown = true;
//            _cooldownTimer = _cooldownTime;
//            _cooldownImage.fillAmount = 0f; // Обнуляем индикатор
//        }

//        public void StartCooldownReverseFill()
//        {
//            _isCooldown = true;
//            _cooldownTimer = _cooldownTime;
//            _cooldownImage.fillAmount = 1f;
//        }

//        public bool IsCooldownOver()
//        {
//            return !_isCooldown;
//        }
//    }

//}

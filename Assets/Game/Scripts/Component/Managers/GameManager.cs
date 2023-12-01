using System.Collections;
using DG.Tweening;
using ParkingJamClone.Enums;
using ParkingJamClone.Logic;
using ParkingJamClone.Structs.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParkingJamClone.Components.Manager
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private GameObject _confetti;
        [SerializeField] private float _confettiDelay;
        [SerializeField] private float _levelCompleteDelay;
        [SerializeField] private float _levelFailDelay;
        
        public void SetLevelPaused()
        {
            // todo: level pause element will added
        }

        public void SetLevelCompleted()
        {
            StartCoroutine(ICompleteLevel());
        }
        
        IEnumerator ICompleteLevel()
        {
            yield return new WaitForSeconds(_confettiDelay);
            _confetti.SetActive(true);
            yield return new WaitForSeconds(_levelCompleteDelay);
            RayHitDetectorManager.ClearList();
            DOTween.KillAll();
            LoadNextLevel();
        }
        
        public void SetLevelFailed()
        {
            StartCoroutine(IReloadLevel());
        }
        
        IEnumerator IReloadLevel()
        {
            yield return new WaitForSeconds(_levelFailDelay);
            RayHitDetectorManager.ClearList();
            ReloadLevel();
        }
        
        private void LoadNextLevel()
        {
            string nextLevel = ProgressManager.Instance.GetNextLevelName();
            EventManager.TriggerEvent(new LevelEvent(LevelState.Completed));
            SceneManager.LoadScene(nextLevel);
        }

        private void ReloadLevel()
        {
            string currLevel = ProgressManager.Instance.GetCurrentLevelName();
            EventManager.TriggerEvent(new LevelEvent(LevelState.Failed));
            SceneManager.LoadScene(currLevel);
        }
    }
}

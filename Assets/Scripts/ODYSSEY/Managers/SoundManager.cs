using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Odyssey;

public class SoundManager : MonoBehaviour, IRequiresContext
{
    IMomentumContext _c;

    public void Init(IMomentumContext context)
    {
        this._c = context;
    }

    void OnEnable()
    {
        _c.Get<IReactBridge>().ToggleAllSound_Event += OnToggleAllSound;
        _c.Get<IReactBridge>().TurnAllSoundOn_Event += OnTurnSoundOn;
        _c.Get<IReactBridge>().TurnAllSoundOff_Event += OnTurnSoundOff;
        _c.Get<IReactBridge>().OnSetVolume_Event += OnSetVolume;
    }

    public void OnDisable()
    {
        _c.Get<IReactBridge>().ToggleAllSound_Event -= OnToggleAllSound;
        _c.Get<IReactBridge>().TurnAllSoundOn_Event -= OnTurnSoundOn;
        _c.Get<IReactBridge>().TurnAllSoundOff_Event -= OnTurnSoundOff;
        _c.Get<IReactBridge>().OnSetVolume_Event -= OnSetVolume;
    }

    void OnTurnSoundOff()
    {
        Logging.Log("[Sound Manager] - got turn sound off event");
        _c.Get<ISessionData>().MutedSound = true;
        AudioListener.pause = _c.Get<ISessionData>().MutedSound;
    }

    void OnTurnSoundOn()
    {
        Logging.Log("[Sound Manager] - got turn sound on event");
        _c.Get<ISessionData>().MutedSound = false;
        AudioListener.pause = _c.Get<ISessionData>().MutedSound;
    }

    void OnToggleAllSound()
    {
        Logging.Log("[Sound Manager] - got toggle sound event");
        _c.Get<ISessionData>().MutedSound = !_c.Get<ISessionData>().MutedSound;
        AudioListener.pause = _c.Get<ISessionData>().MutedSound;        
    }

    void OnSetVolume( string volumeString )
    {
        float volume = float.Parse(volumeString);

        if (volume < 0 || volume > 1)
        {
            Logging.Log("[Sound Manager] - got an invalid set volume event, volume = " + volume);
        }
        else
        {
            Logging.Log("[Sound Manager] - got set volume event, volume = " + volume);
            _c.Get<ISessionData>().SoundVolume = volumeString;
            AudioListener.volume = volume;
            HS.AudioManager.SetGlobalVolume(volume);
        }
    }
}

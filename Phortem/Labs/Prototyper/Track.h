/******************************************************************************
 * Track.h                                                                    *
 ******************************************************************************
 * Project      : DevLib                                                      *
 * License      : LGPL (full notice can be found at root directory)           *
 * Created by   : Arnaud Storq (norecess@devlib-central.org)                  *
 ******************************************************************************/
#ifndef __DEVLIB_TRACK_H__
#define __DEVLIB_TRACK_H__

// ---------------------------------------------------------------------------- TRACK TYPE
enum TrackType
{
    TRACK_TYPE_LINEAR,
    TRACK_TYPE_SPLINE
};

// ---------------------------------------------------------------------------- TRACK KEY
/** Represent a track's key. */
struct TrackKey
{
    double value;
    double time;
};

// ---------------------------------------------------------------------------- TRACK
/** Interface to represent a track. */
class Track
{
public:
    virtual ~Track( );

	static Track *createTrack( TrackType type = TRACK_TYPE_SPLINE );
    
    void setType( TrackType type );
    TrackType getType( );
    
    int getKeyCount( );

    void setKey( TrackKey &key );
    TrackKey *getKey( unsigned int index );
    TrackKey *getKey( double time );

    void deleteKey( unsigned int index );
    void deleteKey( double time );
    void deleteAllKeys( );    
    
    double getValue( double time );

private:
	Track( TrackType type );

    struct TrackKeyChain : public TrackKey
    {
        TrackKeyChain *next;
        TrackKeyChain *prev;

        TrackKeyChain( TrackKey key )
        {
            value = key.value;
            time = key.time;
            
            next = 0;
            prev = 0;
        }
    };

    TrackType m_type;

    TrackKeyChain *m_root;
};

// ---------------------------------------------------------------------------- END
#endif // __DEVLIB_TRACK_H__

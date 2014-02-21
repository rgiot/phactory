

#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <string.h>

#include <SDL.h>

#include "Track.h"

// ---------------------------------------------------------------------------- CONSTRUCTOR
Track::Track( TrackType type )
{
    m_type = type;
    
    m_root = 0;
}

// ---------------------------------------------------------------------------- DESTRUCTOR
Track::~Track( )
{
    deleteAllKeys( );
}

// ---------------------------------------------------------------------------- CREATE TRACK
Track *Track::createTrack( TrackType type )
{
    Track *track = new Track( type );

    return track;
}

// ---------------------------------------------------------------------------- SET TYPE
void Track::setType( TrackType type )
{
    m_type = type;
}

// ---------------------------------------------------------------------------- GET TYPE
TrackType Track::getType( )
{
    return m_type;
}

// ---------------------------------------------------------------------------- SET KEY
void Track::setKey( TrackKey &key )
{
    if ( m_root == 0 )
    {
        m_root = new TrackKeyChain( key );
        return;
    }

    TrackKeyChain *newKeyChain = new TrackKeyChain( key );
                
    TrackKeyChain *keyChain = m_root;

    while ( keyChain != 0 )
    {
        if ( keyChain->time == key.time )
        {
            // replace
            if ( keyChain->prev != 0 )
            {
                keyChain->prev->next = newKeyChain;
            }
            if ( keyChain->next != 0 )
            {
                keyChain->next->prev = newKeyChain;
            }

            newKeyChain->prev = keyChain->prev;
            newKeyChain->next = keyChain->next;

            if ( newKeyChain->prev == 0 )
            {
                m_root = newKeyChain;
            }

            delete keyChain;            
            return;
        }
        else if ( keyChain->time > key.time )
        {
            // insert
            newKeyChain->prev = keyChain->prev;
            newKeyChain->next = keyChain;

            if ( keyChain->prev != 0 )
            {
                keyChain->prev->next = newKeyChain;
            }
            else
            {
                m_root = newKeyChain;
            }

            return;
        }

        if ( keyChain->next == 0 )
        {
            keyChain->next = newKeyChain;
            newKeyChain->prev = keyChain;

            return;
        }
        
        keyChain = keyChain->next;
    }
}

// ---------------------------------------------------------------------------- DELETE KEY ( WITH INDEX )
void Track::deleteKey( unsigned int index )
{
    TrackKeyChain *keyChain = m_root;

    unsigned int count = 0;
    
    while ( keyChain != 0 )
    {
        if ( count == index )
        {
            if ( keyChain->prev == 0 )
            {
                m_root = keyChain->next;
            }
            else
            {
                keyChain->prev->next = keyChain->next;
            }

            if ( keyChain->next != 0 )
            {
                keyChain->next->prev = keyChain->prev;
            }

            delete keyChain;
            return;
        }
        
        count++;
        keyChain = keyChain->next;
    }
}

// ---------------------------------------------------------------------------- DELETE KEY ( WITH TIME )
void Track::deleteKey( double time )
{
    TrackKeyChain *keyChain = m_root;

    while ( keyChain != 0 )
    {
        if ( keyChain->time == time )
        {
            if ( keyChain->prev == 0 )
            {
                m_root = keyChain->next;
            }
            else
            {
                keyChain->prev->next = keyChain->next;
            }

            if ( keyChain->next != 0 )
            {
                keyChain->next->prev = keyChain->prev;
            }

            delete keyChain;
            return;
        }
        
        keyChain = keyChain->next;
    }
}

// ---------------------------------------------------------------------------- DELETE ALL KEYS
void Track::deleteAllKeys( )
{
    TrackKeyChain *keyChain = m_root;

    while ( keyChain != 0 )
    {
        TrackKeyChain *next = keyChain->next;
        delete keyChain;
        
        keyChain = next;
    }

    m_root = 0;
}

// ---------------------------------------------------------------------------- GET KEY COUNT
int Track::getKeyCount( )
{
    TrackKeyChain *keyChain = m_root;

    int count = 0;
    
    while ( keyChain->next != 0 )
    {
        count++;

        keyChain = keyChain->next;
    }

    return count;
}

// ---------------------------------------------------------------------------- GET KEY ( WITH INDEX )
TrackKey *Track::getKey( unsigned int index )
{
    TrackKeyChain *keyChain = m_root;

    unsigned int count = 0;
    
    while ( keyChain->next != 0 )
    {
        if ( count == index )
        {
            return ( TrackKey * ) keyChain;
        }
        
        count++;

        keyChain = keyChain->next;
    }

    return 0;
}

// ---------------------------------------------------------------------------- GET KEY ( WITH TIME )
TrackKey *Track::getKey( double time )
{
    TrackKeyChain *keyChain = m_root;

    int count = 0;
    
    while ( keyChain->next != 0 )
    {
        if ( keyChain->time == time )
        {
            return ( TrackKey * ) keyChain;
        }
        
        count++;

        keyChain = keyChain->next;
    }

    return 0;
}

// ---------------------------------------------------------------------------- GET VALUE
double Track::getValue( double time )
{
    // check if enough keys
    int keyCount = getKeyCount( );
    
    if ( keyCount == 0 )
    {
        return 0.0f;
    }
    if ( keyCount == 1 )
    {
        return m_root->value;
    }

    TrackKeyChain *startKey = 0;
    TrackKeyChain *endKey = 0;

    // get start / end keys
    endKey = m_root;
    startKey = endKey;

    while ( endKey->next != 0 )
    {
        endKey = endKey->next;
    }

    // check boundaries
    if ( time < startKey->time )
    {
        return startKey->value;        
    }
    else if ( time > endKey->time )
    {
        return endKey->value;
    }

    // get the two keys around the specified time
    TrackKeyChain *key0 = m_root;
    while ( time > key0->next->time )
    {
        key0 = key0->next;
    }
    TrackKeyChain *key1 = key0->next;

    // if time is equal to a key, returns the corresponding value
    if ( time == key0->time )
    {
        return key0->value;
    }
    else if ( time == key1->time )
    {
        return key1->value;
    }

    // compute interpoled value
    double t = ( time - key0->time ) / ( key1->time - key0->time );
    
    if ( m_type == TRACK_TYPE_SPLINE )
    {
        double d = key1->value - key0->value;
    
        double in = d;
        double out = d;
        
        if ( key0->prev )
        {
            out = ( key1->time - key0->time ) / ( key1->time - key0->prev->time ) * ( key0->value - key0->prev->value + d );
        }

        if ( key1->next )
        {
            in = ( ( key1->time - key0->time ) / ( key1->next->time - key0->time ) * ( key1->next->value - key1->value + d ) );
        }

        // hermite
        double t2 = t * t;
        double t3 = t * t2;

        double h2 = 3.0f * t2 - t3 - t3;
        double h1 = 1.0f - h2;
        double h4 = t3 - t2;
        double h3 = h4 - t2 + t;

        return h1 * key0->value + h2 * key1->value + h3 * out + h4 * in;
    }
    else if ( m_type == TRACK_TYPE_LINEAR )
    {
        return key0->value + t * ( key1->value - key0->value );
    }

    return 0.0f;
}

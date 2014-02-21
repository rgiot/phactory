
#include "config.h"

#include "fxmanager.h"
#include "fxbase.h"

static FXManager s_fxManager;

FXManager *GetFXManager()
{
	return &s_fxManager;
}

FXManager::FXManager() :
	m_current( 0 )
{
}

FXManager::~FXManager()
{
	Release();
}

void FXManager::RegisterFX( FXBase *fx )
{
	printf("Registering ");
	printf(fx->GetName());
	printf(" (");
	printf(SDL_GetKeyName(fx->GetKey()));
	printf(" key)\n");
		
	m_fxs.push_back( fx );

	if ( m_fxs.size() == 1 )
	{
		m_current = fx;

		InitFX();
	}
}

void FXManager::KeyPress( SDLKey key )
{
	for (unsigned int iFX = 0; iFX < m_fxs.size(); iFX++ )
	{
		if ( m_fxs[ iFX ]->GetKey() == key )
		{
			DestroyFX();

			m_current = m_fxs[ iFX ];
			
			InitFX();

			return;
		}
	}
}

void FXManager::InitFX()
{
	printf("Init ");
	printf(m_current->GetName());
	printf("\n");
		
	m_current->Init();
}

void FXManager::DrawFX()
{
	m_current->Draw();
}

void FXManager::DestroyFX()
{
	printf("Destroy ");
	printf(m_current->GetName());
	printf("\n");
	
	m_current->Destroy();
}

void FXManager::Release()
{
	printf("Releasing FXManager\n");
	
	if ( m_current )
	{
		DestroyFX();
		m_current = 0;
	}
			
	for (unsigned int iFX = 0; iFX < m_fxs.size(); iFX++ )
	{
		delete m_fxs[ iFX ];
	}

	m_fxs.clear();
	m_current = 0;
}

	
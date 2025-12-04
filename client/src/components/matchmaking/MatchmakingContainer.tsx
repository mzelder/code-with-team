import { useState, useEffect } from 'react';
import { getLobbyStatus } from '../../apiClient/matchmaking/matchmaking';
import TeamSelectionView from './TeamSelectionView';
import LobbyComponent from '../lobby/LobbyComponent';

const MatchmakingState = {
    InQueue: 0,
    Canceled: 1,
    FoundLobby: 2
} as const;
type MatchmakingState = typeof MatchmakingState[keyof typeof MatchmakingState];

function MatchmakingContainer() {
    const [matchmakingState, setMatchmakingState] = useState<MatchmakingState>(MatchmakingState.Canceled);

    // this will be happening till lobby will create
    const checkLobbyStatus = async () => {
        try {
            const lobbyStatus = await getLobbyStatus();

            if (!lobbyStatus.found || !lobbyStatus.repositoryUrl) {
                setMatchmakingState(MatchmakingState.Canceled);
                return;
            }
            
            setMatchmakingState(MatchmakingState.FoundLobby)
        } catch (error) {
            console.log("Error while polling:", error);
            setMatchmakingState(MatchmakingState.Canceled);
        }
    }

    const handleQueueStateChange = (isQueuing: boolean) => {
        setMatchmakingState(isQueuing ? MatchmakingState.InQueue : MatchmakingState.Canceled);
    }
    
    useEffect(() => {
        if (matchmakingState === MatchmakingState.InQueue)
            checkLobbyStatus()
    }, [matchmakingState]);

    const renderCurrentState = () => {
        switch (matchmakingState) {
            case MatchmakingState.InQueue:
            case MatchmakingState.Canceled:
                return <TeamSelectionView onQueueStateChange={handleQueueStateChange} />
            case MatchmakingState.FoundLobby:
                return <LobbyComponent />
        }
    }

    return (
        <div>{renderCurrentState()}</div>
    );
}

export default MatchmakingContainer;

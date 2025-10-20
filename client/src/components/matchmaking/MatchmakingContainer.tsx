import { useState, useEffect } from 'react';
import { getLobbyStatus } from '../../apiClient/matchmaking/matchmaking';
import type { LobbyStatusDto } from '../../apiClient/matchmaking/dtos';
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
    const [lobbyData, setLobbyData] = useState<LobbyStatusDto | null>(null);

    const checkLobbyStatus = async () => {
        try {
            const lobbyStatus = await getLobbyStatus();

            if (!lobbyStatus.found) {
                setMatchmakingState(MatchmakingState.Canceled);
                setLobbyData(null);
                return;
            }
            
            setMatchmakingState(MatchmakingState.FoundLobby)
            setLobbyData(lobbyStatus);
        } catch (error) {
            console.log("Error while polling:", error);
            setMatchmakingState(MatchmakingState.Canceled);
            setLobbyData(null);
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
                return <LobbyComponent lobbyData={lobbyData} />
            
        }
    }

    return (
        <div>{renderCurrentState()}</div>
    );
}

export default MatchmakingContainer;

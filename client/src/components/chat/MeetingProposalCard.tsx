import { type MeetingProposalDto, MeetingProposalStatus } from "../../apiClient/chat/dtos";
import { formatProposalDate } from "../../apiClient/chat/formatDates";
import UserAvatar from "../shared/UserAvatar";

interface MeetingProposalCardProps {
    proposal: MeetingProposalDto;
    currentUser: string;
    onVote: (proposalId: number, accept: boolean) => void;
}

function MeetingProposalCard({ proposal, currentUser, onVote }: MeetingProposalCardProps) {
    const userVote = proposal.votes.find(v => v.username === currentUser);
    const acceptedVotes = proposal.votes.filter(v => v.isAccepted === true);
    const rejectedVotes = proposal.votes.filter(v => v.isAccepted === false);

    

    const getCardBorderClass = () => {
        switch (proposal.status) {
            case MeetingProposalStatus.Accepted:
                return "border-green-500";
            case MeetingProposalStatus.Rejected:
                return "border-red-500";
            default:
                return "border-gray-700";
        }
    };

    const getStatusInfo = () => {
        switch (proposal.status) {
            case MeetingProposalStatus.Accepted:
                return { text: "Accepted", color: "text-green-400" };
            case MeetingProposalStatus.Rejected:
                return { text: "Rejected", color: "text-red-400" };
            default:
                return { text: "Pending", color: "text-yellow-400" };
        }
    };

    const canVote = proposal.status === MeetingProposalStatus.Pending && !userVote;
    const statusInfo = getStatusInfo();

    return (
        <div className={`bg-gradient-to-br from-gray-800 to-gray-900 border-2 ${getCardBorderClass()} rounded-xl p-5 mx-4 shadow-lg hover:shadow-xl transition-shadow`}>
            <div className="flex items-center justify-between gap-3 mb-4 pb-3 border-b border-gray-700">
                <div className="flex items-center gap-3">
                    <div className="bg-blue-500/20 p-2 rounded-lg">
                        <svg 
                            className="w-6 h-6 text-blue-400" 
                            fill="none" 
                            stroke="currentColor" 
                            viewBox="0 0 24 24"
                        >
                            <path 
                                strokeLinecap="round" 
                                strokeLinejoin="round" 
                                strokeWidth={2} 
                                d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" 
                            />
                        </svg>
                    </div>
                    <div>
                        <span className="font-bold text-blue-400 text-lg block">Meeting Proposal</span>
                        <span className="text-xs text-gray-400">Proposed by {proposal.username}</span>
                    </div>
                </div>
                <span className={`text-sm font-semibold ${statusInfo.color}`}>
                    {statusInfo.text}
                </span>
            </div>
            
            <div className="mb-4 bg-gray-950/50 border border-gray-700 rounded-lg p-4">
                <div className="flex items-center gap-2 mb-2">
                    <div className="w-2 h-2 bg-blue-500 rounded-full animate-pulse"></div>
                    <p className="text-xs text-gray-400 uppercase tracking-wider">Scheduled Time</p>
                </div>
                <p className="text-xl font-bold text-white mb-2">
                    {formatProposalDate(proposal.meetingTime)}
                </p>
                <p className="text-xs text-gray-500">
                    Created {formatProposalDate(proposal.createdAt)}
                </p>
            </div>

            <div className="space-y-3 mb-4">
                <div className="bg-gray-950/30 rounded-lg p-3 border border-gray-700/50">
                    <div className="flex items-center gap-2 mb-2">
                        <div className="w-3 h-3 bg-green-500 rounded-full"></div>
                        <span className="text-green-400 font-bold text-sm">
                            Accepted ({acceptedVotes.length})
                        </span>
                    </div>
                    {acceptedVotes.length > 0 ? (
                        <div className="flex flex-wrap gap-2 mt-2">
                            {acceptedVotes.map((vote, index) => (
                                <div
                                    key={index}
                                    className="group relative"
                                    title={vote.username}
                                >
                                    <div className="border-2 border-green-500 shadow-lg shadow-green-500/30 rounded-full">
                                        <UserAvatar userName={vote.username} width={36} />
                                    </div>
                                    <div className="absolute bottom-full left-1/2 transform -translate-x-1/2 mb-2 px-2 py-1 bg-gray-900 text-white text-xs rounded opacity-0 group-hover:opacity-100 transition-opacity whitespace-nowrap pointer-events-none border border-gray-700 z-10">
                                        {vote.username}
                                    </div>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="text-gray-500 text-xs italic">No votes yet</p>
                    )}
                </div>

                <div className="bg-gray-950/30 rounded-lg p-3 border border-gray-700/50">
                    <div className="flex items-center gap-2 mb-2">
                        <div className="w-3 h-3 bg-red-500 rounded-full"></div>
                        <span className="text-red-400 font-bold text-sm">
                            Declined ({rejectedVotes.length})
                        </span>
                    </div>
                    {rejectedVotes.length > 0 ? (
                        <div className="flex flex-wrap gap-2 mt-2">
                            {rejectedVotes.map((vote, index) => (
                                <div
                                    key={index}
                                    className="group relative"
                                    title={vote.username}
                                >
                                    <div className="border-2 border-red-500 shadow-lg shadow-red-500/30 rounded-full">
                                        <UserAvatar userName={vote.username} width={36} />
                                    </div>
                                    <div className="absolute bottom-full left-1/2 transform -translate-x-1/2 mb-2 px-2 py-1 bg-gray-900 text-white text-xs rounded opacity-0 group-hover:opacity-100 transition-opacity whitespace-nowrap pointer-events-none border border-gray-700 z-10">
                                        {vote.username}
                                    </div>
                                </div>
                            ))}
                        </div>
                    ) : (
                        <p className="text-gray-500 text-xs italic">No votes yet</p>
                    )}
                </div>
            </div>

            {userVote ? (
                <div className={`rounded-lg p-4 text-center border ${
                    userVote.isAccepted 
                        ? 'bg-green-500/10 border-green-500/30 text-green-400' 
                        : 'bg-red-500/10 border-red-500/30 text-red-400'
                }`}>
                    <div className="flex items-center justify-center gap-2">
                        <div className={`w-2 h-2 rounded-full ${userVote.isAccepted ? 'bg-green-500' : 'bg-red-500'}`}></div>
                        <span className="font-semibold">
                            Your vote: {userVote.isAccepted ? "Accepted" : "Declined"}
                        </span>
                    </div>
                </div>
            ) : canVote ? (
                <div className="flex gap-3">
                    <button
                        onClick={() => onVote(proposal.id, true)}
                        className="flex-1 bg-gradient-to-r from-green-600 to-green-500 hover:from-green-500 hover:to-green-400 text-white py-3 px-4 rounded-lg font-semibold transition-all shadow-lg hover:shadow-green-500/50 border border-green-400/30"
                    >
                        <span className="flex items-center justify-center gap-2">
                            <span>✓</span>
                            <span>Accept</span>
                        </span>
                    </button>
                    <button
                        onClick={() => onVote(proposal.id, false)}
                        className="flex-1 bg-gradient-to-r from-red-600 to-red-500 hover:from-red-500 hover:to-red-400 text-white py-3 px-4 rounded-lg font-semibold transition-all shadow-lg hover:shadow-red-500/50 border border-red-400/30"
                    >
                        <span className="flex items-center justify-center gap-2">
                            <span>✕</span>
                            <span>Decline</span>
                        </span>
                    </button>
                </div>
            ) : null}
        </div>
    );
}

export default MeetingProposalCard;
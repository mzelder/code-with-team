import Button from "../shared/Button";

type FeedbackPanelProps = {
  mentorFeedback: string | null;
  title?: string;
  onClose?: () => void;
};

function FeedbackPanel({ mentorFeedback, title = "Feedback", onClose }: FeedbackPanelProps) {
  return (
    <div
      className="fixed inset-0 flex items-center justify-center z-50 backdrop-blur-sm bg-black/30"
      onClick={onClose}
    >
      <div
        className="bg-[#1F2937] rounded-lg shadow-2xl border border-[#4B5563] w-[90vw] h-[60vh] max-w-3xl overflow-hidden flex flex-col"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="flex items-center justify-between px-4 py-3 border-b border-[#4B5563]">
          <h2 className="text-white text-xl font-semibold">{title}</h2>
          {onClose && (
            <Button
              text="Close"
              onToggle={onClose}
              defaultBorderColor="white"
              defaultTextColor="white"
              className="px-3 py-1 text-sm"
            />
          )}
        </div>

        <div className="flex-1 min-w-0 p-6 bg-[#374151] flex flex-col h-full">
          <textarea
            className="flex-1 min-h-[200px] w-full mt-3 p-3 rounded-md bg-[#1F2937] text-white border border-[#4B5563] focus:outline-none resize-none overflow-auto"
            value={mentorFeedback ?? ""}
            readOnly
            placeholder="No feedback from mentor yet."
          />
        </div>
      </div>
    </div>
  );
}

export default FeedbackPanel;
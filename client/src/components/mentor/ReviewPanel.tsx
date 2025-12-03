import { useState } from "react";
import Markdown from "react-markdown";
import remarkGfm from "remark-gfm";
import Button from "../shared/Button";

type ReviewPanelProps = {
  onClose?: () => void;
  onSendFeedback: (text: string) => void;
  title?: string;
  aiReviewMarkdown?: string
};

function ReviewPanel({
  onClose,
  onSendFeedback,
  title = "Review Panel",
  aiReviewMarkdown
}: ReviewPanelProps) {
  const [feedback, setFeedback] = useState<string>("");
  const showLoading = !aiReviewMarkdown

  return (
    <div
      className="fixed inset-0 flex items-center justify-center z-50 backdrop-blur-sm bg-black/30"
      onClick={onClose}
    >
      <div
        className="bg-[#1F2937] rounded-lg shadow-2xl border border-[#4B5563] w-[90vw] h-[80vh] max-w-6xl overflow-hidden flex flex-col"
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

        <div className="flex flex-row flex-1 min-h-0">
          {/* Left: AI Review as README */}
          <div className="flex-1 min-w-0 p-6 bg-[#374151] border-r border-[#4B5563]">
            {showLoading ? (
              <div className="flex h-full items-center justify-center">
                <div className="flex flex-col items-center gap-4 text-gray-300">
                  <div className="w-12 h-12 border-4 border-[#00D1FF] border-t-transparent rounded-full animate-spin" aria-label="Loading" />
                  <p className="text-sm text-center max-w-xs">
                    Loading review data… If this takes more than a few seconds, try refreshing the page.
                  </p>
                </div>
              </div>
            ) : (
              <div className="prose prose-invert max-w-none h-full overflow-y-auto no-scrollbar">
                <Markdown remarkPlugins={[remarkGfm]}>{aiReviewMarkdown}</Markdown>
              </div>
            )}
          </div>

          {/* Right: Mentor feedback */}
          <div className="flex-1 min-w-0 p-6 bg-[#374151] flex flex-col h-full">
            <label className="text-white text-lg font-medium">Mentor Feedback</label>
            <textarea
              className="flex-1 min-h-[200px] w-full mt-3 p-3 rounded-md bg-[#1F2937] text-white border border-[#4B5563] focus:outline-none focus:ring-2 focus:ring-white resize-none overflow-auto"
              placeholder="Write your feedback for the team..."
              value={feedback}
              onChange={(e) => setFeedback(e.target.value)}
            />
            <div className="flex-none mt-4 pt-3 border-t border-[#4B5563] flex gap-3 justify-end">
              {onClose && (
                <Button
                  text="Cancel"
                  onToggle={onClose}
                  defaultBorderColor="white"
                  defaultTextColor="white"
                />
              )}
              <Button
                text="Send Feedback"
                onToggle={() => onSendFeedback(feedback)}
                isSelected={feedback.trim() !== ""}
                isDisabled={!feedback.trim()}
                defaultBorderColor="white"
                defaultTextColor="white"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ReviewPanel;

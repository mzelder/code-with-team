import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";
import Button from "../shared/Button";
import { type MentorFormDto, type MentorQuestionDto } from "../../apiClient/mentor/dtos";
import { getQuestions, getMentorStatus, submitMentorApplication } from "../../apiClient/mentor/mentor";

export default function MentorForm() {
    const navigate = useNavigate();
    const [questions, setQuestions] = useState<MentorQuestionDto | null>(null);
    const [formData, setFormData] = useState<MentorFormDto>({
        fullname: "",
        email: "",
        motivation: "",
        portfolioLinks: [""],
        scenarioAnswers: [""]
    });

    useEffect(() => {
        const checkMentorStatus = async () => {
            try {
                const mentorStatus = await getMentorStatus();
                if (mentorStatus.appliedForMentor) {
                    navigate("/choose-role");
                    return;
                }
            } catch (error) {
               toast.error("Already applied for mentor");
            }
        };

        const fetchQuestions = async() => {
            const result = await getQuestions();
            setQuestions(result);
        }

        checkMentorStatus();
        fetchQuestions();
    }, []);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    const handleScenarioChange = (index: number, value: string) => {
        const newAnswers = [...formData.scenarioAnswers];
        newAnswers[index] = value;
        setFormData(prev => ({
            ...prev,
            scenarioAnswers: newAnswers
        }));
    };

    const handlePortfolioLinkChange = (index: number, value: string) => {
        const newLinks = [...formData.portfolioLinks];
        newLinks[index] = value;
        setFormData(prev => ({
            ...prev,
            portfolioLinks: newLinks
        }));
    };

    const addPortfolioLink = () => {
        setFormData(prev => ({
            ...prev,
            portfolioLinks: [...prev.portfolioLinks, ""]
        }));
    };

    const removePortfolioLink = (index: number) => {
        if (formData.portfolioLinks.length > 1) {
            const newLinks = formData.portfolioLinks.filter((_, i) => i !== index);
            setFormData(prev => ({
                ...prev,
                portfolioLinks: newLinks
            }));
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        
        if (
            !formData.fullname || 
            !formData.email || 
            !formData.motivation ||
            formData.scenarioAnswers.some(answer => !answer.trim())
        ) {
            toast.error("Please fill in all required fields");
            return;
        }

        try {
            const filteredLinks = formData.portfolioLinks.filter(link => link.trim() !== "");
            
            const submitData = {
                ...formData,
                portfolioLinks: filteredLinks
            };

            await submitMentorApplication(submitData);
            toast.success("Application submitted successfully!");
            navigate("/choose-role");
        } catch (error) {
            toast.error("Failed to submit application");
        }
    };

    if (!questions) {
        return <div className="text-white">Loading...</div>;
    }

    return (
        <div className="min-h-screen bg-gray-900 flex items-center justify-center p-8">
            <div className="w-full max-w-4xl bg-[#374151] border border-white rounded-md p-8">
                <div className="prose prose-invert max-w-none">
                    <h1 className="text-white text-3xl font-bold mb-2">Mentor Application Form</h1>
                    <p className="text-white/90 mb-8">
                        Help shape the next generation of developers by becoming a mentor
                    </p>

                    <form onSubmit={handleSubmit} className="space-y-6">
                        {/* Full Name */}
                        <div>
                            <label htmlFor="fullname" className="block text-white font-medium mb-2">
                                Full Name <span className="text-red-400">*</span>
                            </label>
                            <input
                                type="text"
                                id="fullname"
                                name="fullname"
                                value={formData.fullname}
                                onChange={handleInputChange}
                                className="w-full px-4 py-2 rounded bg-gray-700 text-white border border-gray-600 focus:border-[#00D1FF] focus:outline-none"
                                placeholder="Enter your full name"
                                required
                            />
                        </div>

                        {/* Email */}
                        <div>
                            <label htmlFor="email" className="block text-white font-medium mb-2">
                                Email Address <span className="text-red-400">*</span>
                            </label>
                            <input
                                type="email"
                                id="email"
                                name="email"
                                value={formData.email}
                                onChange={handleInputChange}
                                className="w-full px-4 py-2 rounded bg-gray-700 text-white border border-gray-600 focus:border-[#00D1FF] focus:outline-none"
                                placeholder="your.email@example.com"
                                required
                            />
                        </div>

                        {/* Motivation */}
                        <div>
                            <label htmlFor="motivation" className="block text-white font-medium mb-2">
                                Why do you want to be a mentor? <span className="text-red-400">*</span>
                            </label>
                            <textarea
                                id="motivation"
                                name="motivation"
                                value={formData.motivation}
                                onChange={handleInputChange}
                                rows={5}
                                className="w-full px-4 py-2 rounded bg-gray-700 text-white border border-gray-600 focus:border-[#00D1FF] focus:outline-none resize-none"
                                placeholder="Share your motivation for becoming a mentor..."
                                required
                            />
                        </div>

                        {/* Portfolio Links */}
                        <div>
                            <label className="block text-white font-medium mb-2">
                                Portfolio Links (Optional)
                            </label>
                            <p className="text-white/70 text-sm mb-3">
                                Add links to your portfolio, GitHub, LinkedIn, or other relevant profiles
                            </p>
                            <div className="space-y-3">
                                {formData.portfolioLinks.map((link, index) => (
                                    <div key={index} className="flex gap-2">
                                        <input
                                            type="url"
                                            value={link}
                                            onChange={(e) => handlePortfolioLinkChange(index, e.target.value)}
                                            className="flex-1 px-4 py-2 rounded bg-gray-700 text-white border border-gray-600 focus:border-[#00D1FF] focus:outline-none"
                                            placeholder="https://example.com"
                                        />
                                        {formData.portfolioLinks.length > 1 && (
                                            <Button
                                                text="Remove"
                                                onToggle={() => removePortfolioLink(index)}
                                                className="px-4 py-2 rounded bg-red-600 text-white hover:bg-red-700 transition-colors"
                                                defaultBorderColor="red"
                                                defaultTextColor="red"
                                            />
                                        )}
                                    </div>
                                ))}
                                <Button
                                    onToggle={() => addPortfolioLink()}
                                    className="w-1/4"
                                    isSelected={true}
                                    text = "Add another link"
                                />
                            </div>
                        </div>

                        {/* Scenario Questions */}
                        <div className="space-y-6 pt-4">
                            <h2 className="text-white text-2xl font-semibold">Scenario Questions</h2>
                            <p className="text-white/70">
                                Please answer the following questions to help us understand your mentoring approach
                            </p>

                            {questions.questions.map((question, index) => (
                                <div key={index}>
                                    <label htmlFor={`scenario-${index}`} className="block text-white font-medium mb-2">
                                        {index + 1}. {question} <span className="text-red-400">*</span>
                                    </label>
                                    <textarea
                                        id={`scenario-${index}`}
                                        value={formData.scenarioAnswers[index]}
                                        onChange={(e) => handleScenarioChange(index, e.target.value)}
                                        rows={4}
                                        className="w-full px-4 py-2 rounded bg-gray-700 text-white border border-gray-600 focus:border-[#00D1FF] focus:outline-none resize-none"
                                        placeholder="Share your approach to this situation..."
                                        required
                                    />
                                </div>
                            ))}
                        </div>

                        {/* Submit Buttons */}
                        <div className="flex gap-4 pt-4">
                            <Button
                                text="Cancel"
                                defaultTextColor="white"
                                defaultBorderColor="white"
                                onToggle={() => navigate("/choose-role")}
                                className="flex-1 font-medium text-lg"
                            />
                            <Button
                                text="Submit Application"
                                defaultTextColor="white"
                                isSelected={true}
                                onToggle={() => handleSubmit}
                                className="flex-1 font-medium text-lg"
                            />
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
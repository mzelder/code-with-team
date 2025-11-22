export interface MentorDto {
    isMentor: boolean;
    appliedForMentor: boolean;
}

export interface MentorFormDto {
    fullname: string;
    email: string;
    motivation: string;
    portfolioLinks: string[];
    scenarioAnswers: string[]; 
}

export interface MentorQuestionDto {
    questions: string[];
}
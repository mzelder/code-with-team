import { useState, useEffect } from 'react';

interface DateTimePickerProps {
    onDateSelect: (dateTime: string) => void;
    onClose: () => void;
}

function DateTimePicker({ onDateSelect, onClose }: DateTimePickerProps) {
    const [selectedDate, setSelectedDate] = useState<Date | null>(null);
    const [selectedHour, setSelectedHour] = useState<string>("14");
    const [selectedMinute, setSelectedMinute] = useState<string>("00");

    const today = new Date();
    const maxDate = new Date();
    maxDate.setDate(today.getDate() + 7);

    useEffect(() => {
        const currentHour = today.getHours();
        setSelectedHour(`${currentHour + 1}`);
    }, []);

    const weekDays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const hours = Array.from({ length: 24 }, (_, i) => 
        String(i).padStart(2, '0')
    );
    const minutes = ['00', '15', '30', '45'];

    const getMonthsToShow = (): Date[] => {
        const months: Date[] = [];
        const startMonth = new Date(today.getFullYear(), today.getMonth(), 1);
        const endMonth = new Date(maxDate.getFullYear(), maxDate.getMonth(), 1);

        if (startMonth.getTime() === endMonth.getTime()) {
            months.push(startMonth);
        } else {
            months.push(startMonth);
            months.push(endMonth);
        }

        return months;
    };

    const getDaysInMonth = (date: Date) => {
        const year = date.getFullYear();
        const month = date.getMonth();
        const firstDay = new Date(year, month, 1);
        const lastDay = new Date(year, month + 1, 0);
        const daysInMonth = lastDay.getDate();
        const startingDayOfWeek = firstDay.getDay();

        return { daysInMonth, startingDayOfWeek };
    };

    const isDateValid = (day: number, monthDate: Date): boolean => {
        const date = new Date(
            monthDate.getFullYear(),
            monthDate.getMonth(),
            day
        );
        
        const dateStart = new Date(date);
        dateStart.setHours(0, 0, 0, 0);
        
        const todayStart = new Date(today);
        todayStart.setHours(0, 0, 0, 0);
        
        const maxDateStart = new Date(maxDate);
        maxDateStart.setHours(23, 59, 59, 999);

        return dateStart >= todayStart && dateStart <= maxDateStart;
    };

    const isToday = (day: number, monthDate: Date): boolean => {
        const date = new Date(
            monthDate.getFullYear(),
            monthDate.getMonth(),
            day
        );
        return date.toDateString() === today.toDateString();
    };

    const isSelected = (day: number, monthDate: Date): boolean => {
        if (!selectedDate) return false;
        return (
            selectedDate.getDate() === day &&
            selectedDate.getMonth() === monthDate.getMonth() &&
            selectedDate.getFullYear() === monthDate.getFullYear()
        );
    };

    const handleDayClick = (day: number, monthDate: Date) => {
        if (!isDateValid(day, monthDate)) return;
        
        const date = new Date(
            monthDate.getFullYear(),
            monthDate.getMonth(),
            day
        );
        setSelectedDate(date);
    };

    const handleConfirm = () => {
        if (!selectedDate) return;

        const month = String(selectedDate.getMonth() + 1).padStart(2, '0');
        const day = String(selectedDate.getDate()).padStart(2, '0');
        
        const dateTimeString = `${month}:${day}:${selectedHour}:${selectedMinute}`;
        onDateSelect(dateTimeString);
    };

    const monthsToShow = getMonthsToShow();

    useEffect(() => {
        const handleEsc = (e: KeyboardEvent) => {
            if (e.key === 'Escape') onClose();
        };
        window.addEventListener('keydown', handleEsc);
        return () => window.removeEventListener('keydown', handleEsc);
    }, [onClose]);

    const renderMonth = (monthDate: Date, index: number) => {
        const { daysInMonth, startingDayOfWeek } = getDaysInMonth(monthDate);
        const monthName = monthDate.toLocaleString('default', { 
            month: 'long', 
            year: 'numeric' 
        });

        return (
            <div key={index} className="mb-6">
                <div className="text-center mb-4">
                    <span className="text-white font-medium">{monthName}</span>
                </div>

                <div className="grid grid-cols-7 gap-1 mb-2">
                    {weekDays.map(day => (
                        <div 
                            key={day} 
                            className="text-center text-xs text-gray-400 font-medium py-2"
                        >
                            {day}
                        </div>
                    ))}
                </div>

                <div className="grid grid-cols-7 gap-1">
                    {Array.from({ length: startingDayOfWeek }).map((_, i) => (
                        <div key={`empty-${i}`} className="aspect-square" />
                    ))}

                    {Array.from({ length: daysInMonth }).map((_, i) => {
                        const day = i + 1;
                        const valid = isDateValid(day, monthDate);
                        const selected = isSelected(day, monthDate);
                        const todayDate = isToday(day, monthDate);

                        return (
                            <button
                                key={day}
                                onClick={() => handleDayClick(day, monthDate)}
                                disabled={!valid}
                                className={`
                                    aspect-square rounded-lg flex items-center justify-center text-sm
                                    transition-all duration-200
                                    ${selected 
                                        ? 'bg-[#00D1FF] text-white font-bold shadow-lg shadow-[#00D1FF]/50' 
                                        : valid
                                        ? 'bg-gray-700/80 text-white hover:bg-gray-600/80 hover:scale-105'
                                        : 'bg-gray-900/50 text-gray-600 cursor-not-allowed'
                                    }
                                    ${todayDate && !selected ? 'ring-2 ring-[#00D1FF]' : ''}
                                `}
                            >
                                {day}
                            </button>
                        );
                    })}
                </div>
            </div>
        );
    };

    return (
        <div 
            className="fixed inset-0 flex items-center justify-center z-50 backdrop-blur-sm bg-black/30"
            onClick={onClose}
        >
            <div 
                className={`bg-gray-800/95 rounded-lg shadow-2xl p-6 max-w-full border border-gray-700/50 ${
                    monthsToShow.length === 2 ? 'w-[800px]' : 'w-96'
                }`}
                onClick={(e) => e.stopPropagation()}
            >
                <div className="flex justify-between items-center mb-4">
                    <h3 className="text-xl font-semibold text-white">
                        Select Date & Time
                    </h3>
                    <button
                        onClick={onClose}
                        className="text-gray-400 hover:text-white transition-colors"
                    >
                        <svg 
                            className="w-6 h-6" 
                            fill="none" 
                            stroke="currentColor" 
                            viewBox="0 0 24 24"
                        >
                            <path 
                                strokeLinecap="round" 
                                strokeLinejoin="round" 
                                strokeWidth={2} 
                                d="M6 18L18 6M6 6l12 12" 
                            />
                        </svg>
                    </button>
                </div>

                <div className={`${monthsToShow.length === 2 ? 'grid grid-cols-2 gap-8' : ''}`}>
                    {monthsToShow.map((monthDate, index) => renderMonth(monthDate, index))}
                </div>

                <div className="mb-6">
                    <label className="block text-white text-sm font-medium mb-2">
                        Time
                    </label>
                    <div className="flex gap-2 items-center">
                        <select
                            value={selectedHour}
                            onChange={(e) => setSelectedHour(e.target.value)}
                            className="flex-1 bg-gray-700/80 text-white rounded-lg px-3 py-2 outline-none focus:ring-2 focus:ring-[#00D1FF] transition-all"
                        >
                            {hours.map(hour => (
                                <option key={hour} value={hour}>
                                    {hour}
                                </option>
                            ))}
                        </select>

                        <span className="text-white text-xl">:</span>

                        <select
                            value={selectedMinute}
                            onChange={(e) => setSelectedMinute(e.target.value)}
                            className="flex-1 bg-gray-700/80 text-white rounded-lg px-3 py-2 outline-none focus:ring-2 focus:ring-[#00D1FF] transition-all"
                        >
                            {minutes.map(minute => (
                                <option key={minute} value={minute}>
                                    {minute}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                {selectedDate && (
                    <div className="mb-4 p-3 bg-gray-700/60 rounded-lg backdrop-blur-sm border border-gray-600/30">
                        <p className="text-sm text-gray-400 mb-1">Selected:</p>
                        <p className="text-white font-medium">
                            {selectedDate.toLocaleDateString('en-US', { 
                                month: 'long', 
                                day: 'numeric',
                                year: 'numeric'
                            })} at {selectedHour}:{selectedMinute}
                        </p>
                    </div>
                )}

                <div className="flex gap-2">
                    <button
                        onClick={onClose}
                        className="flex-1 bg-gray-700/80 hover:bg-gray-600/80 text-white font-semibold rounded-lg px-4 py-2 transition-all hover:scale-105"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={handleConfirm}
                        disabled={!selectedDate}
                        className="flex-1 bg-[#00D1FF] hover:bg-[#00B8E6] disabled:bg-gray-600/50 disabled:cursor-not-allowed text-white font-semibold rounded-lg px-4 py-2 transition-all hover:scale-105 hover:shadow-lg hover:shadow-[#00D1FF]/50"
                    >
                        Confirm
                    </button>
                </div>
            </div>
        </div>
    );
}

export default DateTimePicker;
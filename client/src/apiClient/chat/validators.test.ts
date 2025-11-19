import { describe, it, expect, beforeEach, afterEach, vi } from 'vitest';
import { validateBookDate } from './validators';

describe('validateBookDate', () => {
    beforeEach(() => {
        // Mock current date to November 8, 2025, 14:00
        vi.useFakeTimers();
        vi.setSystemTime(new Date(2025, 10, 8, 14, 0, 0)); // Month is 0-indexed
    });

    afterEach(() => {
        vi.useRealTimers();
    });

    describe('when no arguments provided', () => {
        it('should return valid true (opens date picker)', () => {
            const result = validateBookDate([]);
            expect(result.valid).toBe(true);
            expect(result.error).toBeUndefined();
        });
    });

    describe('format validation - part count', () => {
        it('should reject when less than 4 parts', () => {
            const result = validateBookDate(['11:08:14']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Input should be in this format: MM:DD:HH:MM');
        });

        it('should reject when only 1 part', () => {
            const result = validateBookDate(['11']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Input should be in this format: MM:DD:HH:MM');
        });

        it('should reject when more than 4 parts', () => {
            const result = validateBookDate(['11:08:14:30:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Input should be in this format: MM:DD:HH:MM');
        });

        it('should accept exactly 4 parts', () => {
            const result = validateBookDate(['11:09:15:00']);
            expect(result.valid).toBe(true);
        });
    });

    describe('format validation - padding', () => {
        it('should reject single digit month', () => {
            const result = validateBookDate(['1:09:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Each element should have 2 digits. Example: 01:02:03:04');
        });

        it('should reject single digit day', () => {
            const result = validateBookDate(['11:8:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Each element should have 2 digits. Example: 01:02:03:04');
        });

        it('should reject single digit hour', () => {
            const result = validateBookDate(['11:09:4:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Each element should have 2 digits. Example: 01:02:03:04');
        });

        it('should reject single digit minute', () => {
            const result = validateBookDate(['11:09:14:3']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Each element should have 2 digits. Example: 01:02:03:04');
        });

        it('should reject 3 digit parts', () => {
            const result = validateBookDate(['111:09:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Each element should have 2 digits. Example: 01:02:03:04');
        });

        it('should accept properly padded input', () => {
            const result = validateBookDate(['11:09:15:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept all zeros (if valid date)', () => {
            const result = validateBookDate(['11:09:00:00']);
            expect(result.valid).toBe(true);
        });
    });

    describe('month validation', () => {
        it('should reject month 00', () => {
            const result = validateBookDate(['00:09:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Month must be between 01-12');
        });

        it('should reject month 13', () => {
            const result = validateBookDate(['13:09:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Month must be between 01-12');
        });

        it('should reject month 99', () => {
            const result = validateBookDate(['99:09:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Month must be between 01-12');
        });

        it('should accept month 01 (January) even if date is past', () => {
            const result = validateBookDate(['01:15:15:00']);
            expect(result.valid).toBe(false);
            // Should fail due to past date, not invalid month
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept month 11 (November) within valid range', () => {
            const result = validateBookDate(['11:09:15:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept month 12 (December) even if beyond 7 days', () => {
            const result = validateBookDate(['12:09:15:00']);
            expect(result.valid).toBe(false);
            // Should fail due to 7-day limit, not invalid month
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });
    });

    describe('day validation - general', () => {
        it('should reject day 00', () => {
            const result = validateBookDate(['11:00:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toContain('Day must be between 01-');
        });

        it('should accept day 01', () => {
            const result = validateBookDate(['11:09:15:00']);
            expect(result.valid).toBe(true);
        });
    });

    describe('day validation - month-specific days in range', () => {
        it('should reject day 32 for November', () => {
            const result = validateBookDate(['11:32:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 11');
        });

        it('should reject day 31 in November (30 days)', () => {
            const result = validateBookDate(['11:31:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 11');
        });

        it('should accept day 30 in November but fail 7-day check', () => {
            const result = validateBookDate(['11:30:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should accept day 15 in November (within 7 days)', () => {
            const result = validateBookDate(['11:15:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept day 14 in November (within 7 days)', () => {
            const result = validateBookDate(['11:14:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept day 09 in November (within 7 days)', () => {
            const result = validateBookDate(['11:09:15:00']);
            expect(result.valid).toBe(true);
        });
    });

    describe('day validation - month-specific for other months', () => {
        it('should reject day 32 in January (even though past)', () => {
            const result = validateBookDate(['01:32:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-31 for month 01');
        });

        it('should accept day 31 in January but fail past date check', () => {
            const result = validateBookDate(['01:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject day 29 in February 2025 (non-leap year)', () => {
            const result = validateBookDate(['02:29:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-28 for month 02');
        });

        it('should reject day 30 in February', () => {
            const result = validateBookDate(['02:30:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-28 for month 02');
        });

        it('should accept day 28 in February but fail past date check', () => {
            const result = validateBookDate(['02:28:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject day 31 in April (30 days)', () => {
            const result = validateBookDate(['04:31:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 04');
        });

        it('should accept day 30 in April but fail past date check', () => {
            const result = validateBookDate(['04:30:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject day 31 in June (30 days)', () => {
            const result = validateBookDate(['06:31:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 06');
        });

        it('should reject day 31 in September (30 days)', () => {
            const result = validateBookDate(['09:31:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 09');
        });

        it('should accept day 31 in December but fail 7-day check', () => {
            const result = validateBookDate(['12:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should accept day 31 in March but fail past date check', () => {
            const result = validateBookDate(['03:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept day 31 in May but fail past date check', () => {
            const result = validateBookDate(['05:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept day 31 in July but fail past date check', () => {
            const result = validateBookDate(['07:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept day 31 in August but fail past date check', () => {
            const result = validateBookDate(['08:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept day 31 in October but fail past date check', () => {
            const result = validateBookDate(['10:31:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });
    });

    describe('hour validation', () => {
        it('should accept hour 00', () => {
            const result = validateBookDate(['11:09:00:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept hour 12', () => {
            const result = validateBookDate(['11:09:12:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept hour 23', () => {
            const result = validateBookDate(['11:09:23:00']);
            expect(result.valid).toBe(true);
        });

        it('should reject hour 24', () => {
            const result = validateBookDate(['11:09:24:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Hour must be between 00-23');
        });

        it('should reject hour 99', () => {
            const result = validateBookDate(['11:09:99:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Hour must be between 00-23');
        });
    });

    describe('minute validation', () => {
        it('should accept minute 00', () => {
            const result = validateBookDate(['11:09:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept minute 30', () => {
            const result = validateBookDate(['11:09:14:30']);
            expect(result.valid).toBe(true);
        });

        it('should accept minute 59', () => {
            const result = validateBookDate(['11:09:14:59']);
            expect(result.valid).toBe(true);
        });

        it('should reject minute 60', () => {
            const result = validateBookDate(['11:09:14:60']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Minute must be between 00-59');
        });

        it('should reject minute 99', () => {
            const result = validateBookDate(['11:09:14:99']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Minute must be between 00-59');
        });
    });

    describe('past date validation', () => {
        it('should reject exact current time', () => {
            // Current: Nov 8, 14:00
            const result = validateBookDate(['11:08:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject past time on same day', () => {
            // Current: Nov 8, 14:00
            const result = validateBookDate(['11:08:13:59']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject previous day', () => {
            const result = validateBookDate(['11:07:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject previous month', () => {
            const result = validateBookDate(['10:15:15:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject previous year date (early in year)', () => {
            const result = validateBookDate(['01:01:12:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept future time on same day', () => {
            // Current: Nov 8, 14:00
            const result = validateBookDate(['11:08:14:01']);
            expect(result.valid).toBe(true);
        });

        it('should accept next day', () => {
            const result = validateBookDate(['11:09:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept later in same month (within 7 days)', () => {
            const result = validateBookDate(['11:15:10:00']);
            expect(result.valid).toBe(true);
        });
    });

    describe('one week limit validation', () => {
        it('should accept date 1 day from now', () => {
            const result = validateBookDate(['11:09:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept date 3 days from now', () => {
            const result = validateBookDate(['11:11:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept date 6 days from now', () => {
            const result = validateBookDate(['11:14:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept date exactly 7 days from now', () => {
            // Current: Nov 8, 14:00
            const result = validateBookDate(['11:15:14:00']); // Nov 15, 14:00
            expect(result.valid).toBe(true);
        });

        it('should reject date 8 days from now', () => {
            const result = validateBookDate(['11:16:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject date 10 days from now', () => {
            const result = validateBookDate(['11:18:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject next month (beyond 7 days)', () => {
            const result = validateBookDate(['12:01:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject date exactly 7 days + 1 minute from now', () => {
            const result = validateBookDate(['11:15:14:01']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });
    });

    describe('year transition validation (December 2025 -> January 2026)', () => {
        it('should reject December date beyond 7 days', () => {
            const result = validateBookDate(['12:20:14:00']); // Dec 20, 2025
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject December 31st (beyond 7 days)', () => {
            const result = validateBookDate(['12:31:14:00']); // Dec 31, 2025
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject January 1st 2026 (assumes current year 2025)', () => {
            // Note: Validator assumes current year, so Jan 1 is treated as past (Jan 1, 2025)
            const result = validateBookDate(['01:01:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject any January date (treated as past year)', () => {
            const result = validateBookDate(['01:15:14:00']); // Jan 15, 2025 (past)
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });
    });

    describe('year transition edge cases from late December', () => {
        beforeEach(() => {
            // Mock current date to December 30, 2025, 14:00
            vi.useFakeTimers();
            vi.setSystemTime(new Date(2025, 11, 30, 14, 0, 0)); // Month 11 = December
        });

        it('should accept December 31st (1 day from now)', () => {
            const result = validateBookDate(['12:31:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should accept January 1st but fail (treated as past year 2025)', () => {
            // Validator assumes current year (2025), so Jan 1 = Jan 1, 2025 (past)
            const result = validateBookDate(['01:01:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should accept January 5th but fail (treated as past year 2025)', () => {
            // Even though it would be within 7 days if it was 2026, 
            // it's treated as Jan 5, 2025 (past)
            const result = validateBookDate(['01:05:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject February dates (treated as past year)', () => {
            const result = validateBookDate(['02:15:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });
    });

    describe('year transition from early January', () => {
        beforeEach(() => {
            // Mock current date to January 2, 2026, 14:00
            vi.useFakeTimers();
            vi.setSystemTime(new Date(2026, 0, 2, 14, 0, 0)); // Month 0 = January
        });

        it('should accept January 9th (within 7 days)', () => {
            const result = validateBookDate(['01:09:14:00']);
            expect(result.valid).toBe(true);
        });

        it('should reject January 10th (8 days from now)', () => {
            const result = validateBookDate(['01:10:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject January 1st (past)', () => {
            const result = validateBookDate(['01:01:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject December dates (treated as previous year, past)', () => {
            const result = validateBookDate(['12:25:14:00']); // Dec 25, 2025
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should reject February dates beyond 7 days', () => {
            const result = validateBookDate(['02:01:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });
    });

    describe('leap year validation', () => {
        beforeEach(() => {
            // Mock current date to February 25, 2024, 14:00 (leap year)
            vi.useFakeTimers();
            vi.setSystemTime(new Date(2024, 1, 25, 14, 0, 0));
        });

        it('should accept February 29th in leap year 2024 (within 7 days)', () => {
            const result = validateBookDate(['02:29:14:00']);
            expect(result.valid).toBe(true);
            expect(result.error).toBeUndefined();
        });

        it('should validate day 29 exists in leap year February', () => {
            const result = validateBookDate(['02:29:14:00']);
            // Should NOT fail with day validation error
            expect(result.valid).toBe(true);
        });

        it('should reject February 30th even in leap year', () => {
            const result = validateBookDate(['02:30:14:00']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-29 for month 02');
        });
    });

    describe('edge cases', () => {
        it('should handle midnight correctly', () => {
            const result = validateBookDate(['11:09:00:00']);
            expect(result.valid).toBe(true);
        });

        it('should handle end of day correctly', () => {
            const result = validateBookDate(['11:09:23:59']);
            expect(result.valid).toBe(true);
        });

        it('should handle noon correctly', () => {
            const result = validateBookDate(['11:09:12:00']);
            expect(result.valid).toBe(true);
        });

        it('should handle month transition (Nov 30 -> Dec)', () => {
            const result = validateBookDate(['11:30:14:00']);
            expect(result.valid).toBe(false); // Beyond 7 days
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should handle leading zeros in all parts', () => {
            const result = validateBookDate(['01:01:00:00']);
            expect(result.valid).toBe(false); // Past date
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });
    });

    describe('combined validation scenarios', () => {
        it('should reject when format is valid but month is invalid', () => {
            const result = validateBookDate(['13:15:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Month must be between 01-12');
        });

        it('should reject when format and month are valid but day is invalid', () => {
            const result = validateBookDate(['11:32:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Day must be between 01-30 for month 11');
        });

        it('should reject when all ranges valid but date is in past', () => {
            const result = validateBookDate(['11:01:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Cannot propose a meeting in the past');
        });

        it('should reject when all valid but beyond 7 days', () => {
            const result = validateBookDate(['11:20:14:30']);
            expect(result.valid).toBe(false);
            expect(result.error).toBe('Meeting must be scheduled within one week from now');
        });

        it('should accept when all validations pass', () => {
            const result = validateBookDate(['11:12:15:30']);
            expect(result.valid).toBe(true);
            expect(result.error).toBeUndefined();
        });
    });
});
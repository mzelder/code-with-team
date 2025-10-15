interface CheckBoxProps {
    isChecked: boolean;
}

function CheckBox({ isChecked }: CheckBoxProps) {
    return isChecked ? (
        <svg width="30" height="32" viewBox="0 0 30 32" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect x="3" y="8" width="23" height="23" rx="3" stroke="white" stroke-width="2"/>
            <path fill-rule="evenodd" clip-rule="evenodd" d="M26.7831 0.62349C27.2672 -0.0486205 28.2046 -0.201163 28.8769 0.28267C29.5491 0.766698 29.7017 1.70413 29.2177 2.37642L11.2177 27.3764L11.1718 27.3432C11.1407 27.3802 11.1075 27.4161 11.0722 27.4506C10.4801 28.0299 9.53048 28.0201 8.95109 27.4282L0.427657 18.7163C-0.151638 18.1241 -0.141032 17.1745 0.451094 16.5952C1.04326 16.0161 1.99292 16.0266 2.57219 16.6186L9.88469 24.0922L26.7831 0.62349Z" fill="#42CC67"/>
        </svg>
    ) : (
        <svg width="26" height="25" viewBox="0 0 26 25" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect x="2" y="1.5" width="22" height="22" rx="1.5" stroke="white" stroke-width="3"/>
        </svg>
    );
}

export default CheckBox;
import { motion, useInView } from "motion/react";
import { useEffect, useState, useRef } from "react";

interface Props {
  text: string;
  className?: string;
  speed?: number;
  cursorStyle?: string;
}

export function TypingAnimation({
  text,
  className = "",
  speed = 50,
  cursorStyle = "underscore",
}: Props) {
  const [displayed, setDisplayed] = useState("");
  const ref = useRef<HTMLDivElement>(null);
  const inView = useInView(ref, { once: true });

  useEffect(() => {
    if (!inView) return;
    let i = 0;
    setDisplayed("");
    const interval = setInterval(() => {
      setDisplayed(text.substring(0, i + 1));
      i++;
      if (i >= text.length) clearInterval(interval);
    }, speed);
    return () => clearInterval(interval);
  }, [inView, text, speed]);

  return (
    <div ref={ref} className={className}>
      <span>{displayed}</span>
      <motion.span
        animate={{ opacity: [1, 0] }}
        transition={{ duration: 0.5, repeat: Infinity, repeatType: "reverse" }}
      >
        {cursorStyle === "underscore" ? "_" : "|"}
      </motion.span>
    </div>
  );
}

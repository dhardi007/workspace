import { TypingAnimation } from "./ui/typing-animation";

interface Props {
  text: string;
  class?: string;
}

export default function TypingAnimationWrapper(props: Props) {
  return <TypingAnimation {...props} />;
}
